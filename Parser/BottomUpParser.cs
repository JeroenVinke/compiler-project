using Compiler.Common;
using Compiler.Common.Instructions;
using Compiler.LexicalAnalyer;
using Compiler.Parser.Instances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compiler.Parser
{
    public class BottomUpParser
    {
        private bool DebugModeEnabled = false;
        private LexicalAnalyzer LexicalAnalyzer { get; set; }
        private Token Current { get; set; }
        public SymbolTable RootSymbolTable { get; set; } = new SymbolTable();

        private List<ItemSet> Stack = new List<ItemSet>();
        private ParsingTable ParsingTable { get; set; }
        public SyntaxTreeNode TopLevelAST { get; set; }
        private List<ExpressionDefinition> GrammarSymbols { get; set; }
        private List<ItemSet> CanonicalSets { get; set; }
        private List<ParsingNode> ParsingNodes = new List<ParsingNode>();
        private ItemSet Initial { get; set; }

        private List<Instruction> _il;
        public List<Instruction> IL
        {
            get
            {
                if(_il == null)
                {
                    _il = new List<Instruction>();
                    TopLevelAST.GenerateCode(_il);
                }
                return _il;
            }
        }

        public BottomUpParser() : this(null)
        {
        }

        public BottomUpParser(LexicalAnalyzer lexicalAnalyzer)
        {
            LexicalAnalyzer = lexicalAnalyzer;

            GenerateAutomaton();

            ParsingTable = new ParsingTable(CanonicalSets, GrammarSymbols, Shift, Accept, Reduce);
        }

        public void GenerateAutomaton()
        {
            SubProduction startingRule = Grammar.Instance.First(x => x.Identifier == ParserConstants.Initial).First();
            GrammarSymbols = Grammar.Instance.Symbols();

            Item startingItem = new Item(startingRule, 0, new HashSet<TerminalExpressionDefinition> { new TerminalExpressionDefinition { TokenType = TokenType.EndMarker } });
            Initial = new ItemSet(new List<Item> { startingItem }).Closure();
            CanonicalSets = Initial.GetCanonicalSets(GrammarSymbols);
            PropogateLookaheads(GrammarSymbols);
        }

        public string GetAutomaton()
        {
            return Dotify(CanonicalSets.First());
        }

        public BottomUpParser Parse()
        {
            if (DebugModeEnabled)
            {
                OutputGrammar();
                OutputAutomaton();
            }
            
            Current = LexicalAnalyzer.GetNextToken();

            Stack.Add(Initial);

            DoParse();

            ParsingNodes.Last().EvaluateAttributes();

            TopLevelAST = ParsingNodes.Last().GetAttribute<SyntaxTreeNode>(ParserConstants.SyntaxTreeNode);

            if (DebugModeEnabled)
            {
                OutputDebugFiles();
                OutputIL();
            }

            return this;
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
                                item.AddLookaheads(lookaheads);
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
                ActionParsingTableEntry entry = GetEntry();

                if (!entry.Action(entry))
                {
                    break;
                }
            }
        }

        private ActionParsingTableEntry GetEntry()
        {
            List<ActionParsingTableEntry> entries = ParsingTable.GetSegment(Stack.Last()).Entries
                .Where(x => x is ActionParsingTableEntry a
                       && a.ExpressionDefinition.TokenType == Current.Type).Cast<ActionParsingTableEntry>().ToList();

            if (entries.Count == 1)
            {
                return entries.First();
            }
            else if (entries.Count > 1)
            {
                // reduce/reduce of shift/reduce conflict

                List<ActionParsingTableEntry> entriesForLookahead = entries.Where(x => x.Items.Any(y => y.Lookahead.Any(z => z.TokenType == Current.Type))).ToList(); ;

                if (entriesForLookahead.Count == 1)
                {
                    return entriesForLookahead.First();
                }

                if (entries.Count == 1)
                {
                    return entries.First();
                }
                else if (entries.Count > 1)
                {
                    return ResolveShiftReduceConflicts(entries);
                }
                else
                {
                    Error();
                }
            }
            else
            {
                Error();
            }

            return null;
        }

        private ActionParsingTableEntry ResolveShiftReduceConflicts(List<ActionParsingTableEntry> entries)
        {
            if (entries.Any(x => x.ExpressionDefinition.TokenType == TokenType.Else
                                && x.ActionDescription == "s"))
            {
                return entries.First(x => x.ExpressionDefinition.TokenType == TokenType.Else
                                && x.ActionDescription == "s");
            }

            return null;
        }

        public BottomUpParser OutputGrammar()
        {
            string result = "";
            foreach (Production production in Grammar.Instance)
            {
                result += production + Environment.NewLine;
            }
            File.WriteAllText("grammar.txt", result);

            return this;
        }

        public BottomUpParser OutputAutomaton()
        {
            string result = "";
            result += "digraph A {\r\n";
            result += GetAutomaton();
            result += "}\r\n";
            File.WriteAllText("automaton.txt", result);

            return this;
        }

        public BottomUpParser OutputDebugFiles()
        {

            //result = "";
            //result += "digraph B {\r\n";
            //result += ParsingTable.ToDot(CanonicalSets, GrammarSymbols);
            //result += "}\r\n";
            //File.WriteAllText("parsingtable.txt", result);

            OutputGrammar();
            OutputAutomaton();

            string result = "";
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

            result = "";
            result += "digraph E {\r\n";
            result += RootSymbolTable.ToDot();
            result += "}\r\n";
            File.WriteAllText("symboltable.txt", result);

            return this;
        }

        public BottomUpParser OutputIL()
        {
            string result = GetILAsString();

            File.WriteAllText("IL.txt", result);
            Console.WriteLine(result);

            return this;
        }

        public List<Instruction> GetIL()
        {
            return IL;
        }

        public string GetILAsString()
        {
            string result = "";

            foreach (Instruction instruction in IL)
            {
                string code = instruction.GenerateCodeString();

                if (!string.IsNullOrEmpty(code))
                {
                    result += code + Environment.NewLine;
                }
            }

            return result;
        }

        private void Error()
        {
            //Stack.Last().ToString();
            // error recovery routine
            throw new Exception($"Unexpected token {Current.ToString()} while processing rule");
        }

        private bool Reduce(ActionParsingTableEntry entry)
        {
            SubProduction subProduction = null;

            if (entry.Items.Count > 1)
            {
                Item match = entry.Items.First(x => x.Lookahead.Any(x => x.TokenType == Current.Type));
                subProduction = match.SubProduction;
            }
            else
            {
                subProduction = entry.Items.First().SubProduction;
            }

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
            List<ParsingTableEntry> entries = ParsingTable.GetSegment(tos).Entries
                .Where(x => x is GotoParsingTableEntry g
                && g.ItemSet == tos
                && g.ExpressionDefinition.IsEqualTo(target)).ToList();

            if (entries.Count > 1)
            {
                throw new Exception();
            }

            GotoParsingTableEntry gotoEntry = (GotoParsingTableEntry)entries.First();
            if (DebugModeEnabled)
            {
                Console.WriteLine("REDUCE DEST " + gotoEntry.Destination.Id + ", TARGET " + target.ToString());
            }
            Stack.Add(gotoEntry.Destination);

            return true;
        }

        private bool Accept(ActionParsingTableEntry entry)
        {
            return false;
        }

        private bool Shift(ActionParsingTableEntry entry)
        {
            ItemSet arg1 = entry.ItemSet;
            ExpressionDefinition arg3 = entry.ExpressionDefinition;

            List<ItemSet> transitions = arg1.Transitions.Where(x => x.Key.IsEqualTo(arg3)).Select(x => x.Value).ToList();

            if (transitions.Count > 1)
            {
                throw new Exception();
            }

            ItemSet transition = transitions.First();
            if (DebugModeEnabled)
            {
                Console.WriteLine("SHIFT " + Current.ToString() + ", to" + transition.Id);
            }
            Stack.Add(transition);

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

            parsingNode.Attributes.Add(ParserConstants.Token, Current);

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