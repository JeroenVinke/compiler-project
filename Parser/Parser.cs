using Compiler.Common;
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
        public LexicalAnalyzer LexicalAnalyzer { get; set; }
        private Token Current { get; set; }
        public SymbolTable RootSymbolTable { get; set; } = new SymbolTable();

        private List<ItemSet> Stack = new List<ItemSet>();
        private ParsingTable ParsingTable { get; set; }
        public List<ExpressionDefinition> Symbols { get; private set; }

        public BottomUpParser(LexicalAnalyzer lexicalAnalyzer)
        {
            LexicalAnalyzer = lexicalAnalyzer;
        }

        public void Parse()
        {
            Current = LexicalAnalyzer.GetNextToken();

            Console.WriteLine("------ grammar ------");
            foreach (Production production in Grammar.Instance)
            {
                Console.WriteLine(production);
            }
            Console.WriteLine("------ end of grammar ------");

            SubProduction startingRule = Grammar.Instance.First(x => x.Identifier == "Initial").First();
            Symbols = Grammar.Instance.Symbols();

            Item startingItem = new Item(startingRule, new List<TerminalExpressionDefinition> { new TerminalExpressionDefinition { TokenType = TokenType.EndMarker } });
            ItemSet initial = new ItemSet(new List<Item> { startingItem }).Closure();
            List<ItemSet> C = initial.GetCanonicalSets(Symbols);
            PropogateLookaheads(Symbols, C);

            ParsingTable = ParsingTable.Create(C, Symbols, Shift, Accept, Reduce);

            Stack.Add(initial);

            string result = "";
            result += "digraph A {\r\n";
            result += Dotify(C.First());
            result += "}\r\n";
            result += "digraph B {\r\n";
            result += ParsingTable.ToDot(C, Symbols);
            result += "}\r\n";

            Console.WriteLine(result);

            File.WriteAllText("output.txt", result);

            DoParse();
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

        private void PropogateLookaheads(List<ExpressionDefinition> symbols, List<ItemSet> C)
        {
            List<LookaheadPropogation> propogations = new List<LookaheadPropogation>();

            foreach (ItemSet set in C)
            {
                foreach (ExpressionDefinition ted in symbols)
                {
                    propogations.AddRange(DetermineLookaheads(set, ted));
                }
            }

            bool addedLookahead = true;
            while (addedLookahead)
            {
                addedLookahead = false;
                foreach (ItemSet set in C)
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

        private List<LookaheadPropogation> DetermineLookaheads(ItemSet k, ExpressionDefinition x)
        {
            List<LookaheadPropogation> result = new List<LookaheadPropogation>();

            if (!k.Transitions.Keys.Contains(x))
            {
                return result;
            }

            foreach (Item item in k.KernelItems())
            {
                ItemSet j = new ItemSet(
                    new List<Item>()
                    {
                        new Item(item.SubProduction,
                            new List<TerminalExpressionDefinition>() {
                                new TerminalExpressionDefinition {
                                    TokenType = TokenType.Hash
                                }
                            }
                        )
                    }
                );

                foreach (Item closureItem in j.KernelItems())
                {
                    if (closureItem.ExpressionAfterDot != null
                        && closureItem.ExpressionAfterDot.IsEqualTo(x))
                    {
                        Item i = k.Transitions[x].First(xx => xx.IsEqualTo(closureItem, true));

                        if (!closureItem.Lookahead.Any(y => y.TokenType == TokenType.Hash))
                        {
                            i.Lookahead.AddRange(closureItem.Lookahead.Except(i.Lookahead));
                        }
                        else
                        {
                            result.Add(new LookaheadPropogation
                            {
                                FromItem = item,
                                FromSet = k,
                                ToSet = k.Transitions[x],
                                ToItem = i
                            });
                        }
                    }
                }
            }

            return result;
        }
        
        private void Error()
        {
            // error recovery routine
            throw new Exception("Syntax error");
        }

        private bool Reduce(ItemSet arg1, ExpressionDefinition arg3, SubProduction subProduction)
        {
            NonTerminalExpressionDefinition target = new NonTerminalExpressionDefinition {
                Identifier = subProduction.Production.Identifier
            };

            Console.WriteLine("REDUCE " + subProduction.Production.Identifier);

            ItemSet tos = Stack.Last();

            for(int i = 0; i < subProduction.Where(x => !(x is SemanticActionDefinition) && !(x is TerminalExpressionDefinition ted && ted.TokenType == TokenType.EmptyString)).Count(); i++)
            {
                Stack.RemoveAt(Stack.Count - 1);
                tos = Stack.Last();
            }
            tos = Stack.Last();
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
            Console.WriteLine("SHIFT " + arg3.ToString());
            Stack.Add(arg1.Transitions.First(x => x.Key.IsEqualTo(arg3)).Value);
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