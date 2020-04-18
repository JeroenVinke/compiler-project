using Compiler.Common;
using Compiler.LexicalAnalyer;
using Compiler.Parser.Common;
using Compiler.Parser.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compiler.Parser
{
    public class BottomUpParser
    {
        private LexicalAnalyzer LexicalAnalyzer { get; set; }
        private Token Current { get; set; }
        public SymbolTable RootSymbolTable { get; set; } = new SymbolTable();

        private List<ItemSet> Stack = new List<ItemSet>();
        private ParsingTable ParsingTable { get; set; }
        private SyntaxTreeNode TopLevelAST { get; set; }
        private List<ExpressionDefinition> GrammarSymbols { get; set; }
        private List<ItemSet> CanonicalSets { get; set; }

        public BottomUpParser(LexicalAnalyzer lexicalAnalyzer)
        {
            LexicalAnalyzer = lexicalAnalyzer;
        }

        public void Parse()
        {
            Current = LexicalAnalyzer.GetNextToken();

            SubProduction startingRule = Grammar.Instance.First(x => x.Identifier == "Initial").First();
            GrammarSymbols = Grammar.Instance.Symbols();

            Item startingItem = new Item(startingRule, new List<TerminalExpressionDefinition> { new TerminalExpressionDefinition { TokenType = TokenType.EndMarker } });
            ItemSet initial = new ItemSet(new List<Item> { startingItem }).Closure();
            CanonicalSets = initial.GetCanonicalSets(GrammarSymbols);
            PropogateLookaheads(GrammarSymbols);

            ParsingTable = ParsingTable.Create(CanonicalSets, GrammarSymbols, Shift, Accept, Reduce);

            Stack.Add(initial);

            DoParse();

            ParsingNodes.Last().EvaluateAttributes();

            TopLevelAST = ParsingNodes.Last().GetAttribute<SyntaxTreeNode>("syntaxtreenode");
        }



        private void PropogateLookaheads(List<ExpressionDefinition> symbols)
        {
            List<LookaheadPropogation> propogations = new List<LookaheadPropogation>();

            foreach (ItemSet set in CanonicalSets)
            {
                foreach (ExpressionDefinition ted in symbols)
                {
                    propogations.AddRange(set.DetermineLookaheads(ted));
                }
            }

            bool addedLookahead = true;
            while (addedLookahead)
            {
                addedLookahead = false;
                foreach (ItemSet set in CanonicalSets)
                {
                    foreach (Item item in set.KernelItems())
                    {
                        LookaheadPropogation propogation = propogations.FirstOrDefault(x => x.ToSet == set && x.ToItem.IsEqualTo(item));
                        if (propogation != null)
                        {
                            List<TerminalExpressionDefinition> lookaheads = propogation.FromItem.Lookahead.Except(item.Lookahead).ToList();

                            if (lookaheads.Count > 0)
                            {
                                addedLookahead = true;
                                item.Lookahead.AddRange(lookaheads);
                            }
                        }
                    }
                }
            }
        }

        private void DoParse()
        {
            while (true)
            {
                ActionParsingTableEntry entry = ParsingTable.FirstOrDefault(x => x is ActionParsingTableEntry a
                    && a.ItemSet == Stack.Last()
                    && a.ExpressionDefinition.TokenType == Current.Type) as ActionParsingTableEntry;

                if (entry == null)
                {

                    Error();
                }
                else
                {
                    if (!entry.Action(entry.ItemSet, entry.ExpressionDefinition))
                    {
                        break;
                    }
                }
            }
        }

        public void OutputDebugFiles()
        {
            string result = "";
            result += "digraph A {\r\n";
            result += Dotify(CanonicalSets.First());
            result += "}\r\n";
            File.WriteAllText("automaton.txt", result);

            result = "";
            result += "digraph B {\r\n";
            result += ParsingTable.ToDot(CanonicalSets, GrammarSymbols);
            result += "}\r\n";
            File.WriteAllText("parsingtable.txt", result);

            result = "";
            result += "digraph C {\r\n";
            result += TopLevelAST.ToDot();
            result += "}\r\n";
            File.WriteAllText("syntaxtree.txt", result);

            result = "";
            foreach (Production production in Grammar.Instance)
            {
                result += production + Environment.NewLine;
            }
            File.WriteAllText("grammar.txt", result);
        }

        public void OutputIL()
        {
            List<Instruction> instructions = new List<Instruction>();
            TopLevelAST.GenerateCode(instructions);

            string result = "";

            foreach (Instruction instruction in instructions)
            {
                string code = instruction.GenerateCodeString();

                if (!string.IsNullOrEmpty(code))
                {
                    result += code + Environment.NewLine;
                }
            }

            File.WriteAllText("IL.txt", result);
            Console.WriteLine(result);
        }

        private void Error()
        {
            // error recovery routine
            throw new Exception("TODO: Syntax error");
        }

        private List<ParsingNode> ParsingNodes = new List<ParsingNode>();
        private bool Reduce(ItemSet arg1, ExpressionDefinition arg3, SubProduction subProduction)
        {
            NonTerminalExpressionDefinition target = new NonTerminalExpressionDefinition {
                Identifier = subProduction.Production.Identifier
            };

            ParsingNode parsingNode = new ParsingNode()
            {
                Expression = new NonTerminalExpression
                {
                    Identifier = subProduction.Production.Identifier,
                },
                Parser = this,
                SubProduction = subProduction
            };

            for (int y = subProduction.Count - 1; y >= 0; y--)
            {
                for (int i = ParsingNodes.Count - 1; i >= 0; i--)
                {
                    if (ParsingNodes[i].Parent != null)
                    {
                        continue;
                    }

                    if (ParsingNodes[i].Expression is NonTerminalExpression ne
                        && subProduction[y] is NonTerminalExpressionDefinition ned)
                    {
                        if (ne.Identifier == ned.Identifier)
                        {
                            ParsingNodes[i].Expression.Key = ned.Key;
                            ParsingNodes[i].Parent = parsingNode;
                            break;
                        }

                    }
                    if (ParsingNodes[i].Expression is TerminalExpression te
                        && subProduction[y] is TerminalExpressionDefinition ted)
                    {
                        if (te.TokenType == ted.TokenType)
                        {
                            ParsingNodes[i].Parent = parsingNode;
                            break;
                        }
                    }
                }
            }

            ParsingNodes.Add(parsingNode);

            List<ExpressionDefinition> expressionDefinitionsToRemove = subProduction.Where(x => !(x is SemanticActionDefinition) && !(x is TerminalExpressionDefinition ted && ted.TokenType == TokenType.EmptyString)).ToList();
            for (int i = 0; i < expressionDefinitionsToRemove.Count(); i++)
            {
                Stack.RemoveAt(Stack.Count - 1);
            }

            ItemSet tos = Stack.Last();
            GotoParsingTableEntry entry = (GotoParsingTableEntry)ParsingTable.First(x => x is GotoParsingTableEntry g
                && g.ItemSet == tos
                && g.ExpressionDefinition.IsEqualTo(target));
            Stack.Add(entry.Destination);

            return true;
        }

        private bool Accept(ItemSet arg1, ExpressionDefinition arg3)
        {
            return false;
        }

        private bool Shift(ItemSet arg1, ExpressionDefinition arg3)
        {
            Stack.Add(arg1.Transitions.First(x => x.Key.IsEqualTo(arg3)).Value);

            ParsingNode parsingNode = new ParsingNode()
            {
                Expression = new TerminalExpression
                {
                    TokenType = ((TerminalExpressionDefinition)arg3).TokenType,
                    Key = ((TerminalExpressionDefinition)arg3).Key
                },
                Parser = this,
                SubProduction = arg3.SubProduction
            };

            parsingNode.Attributes.Add("token", Current);

            ParsingNodes.Add(parsingNode);

            Current = LexicalAnalyzer.GetNextToken();

            return true;
        }

        public string Dotify(ItemSet cur, List<ItemSet> visited = null)
        {
            string result = "";

            visited = visited ?? new List<ItemSet>();
            visited.Add(cur);

            result += cur.ToDot();

            foreach (KeyValuePair<ExpressionDefinition, ItemSet> transition in cur.Transitions)
            {
                result += $"{cur.Id} -> {transition.Value.Id} [label=\"{transition.Key}\"]";

                if (!visited.Contains(transition.Value))
                {
                    result += Dotify(transition.Value, visited);
                }
            }

            return result;
        }
    }
}