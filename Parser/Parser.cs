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
        public List<ExpressionDefinition> Symbols { get; set; } = new List<ExpressionDefinition>();

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
            List<ExpressionDefinition> symbols = Grammar.Instance.Symbols();

            ItemSet initial = new ItemSet(new List<Item> { new Item(startingRule, new TerminalExpressionDefinition { TokenType = TokenType.EndMarker }) }).Closure();
            List<ItemSet> C = GetCanonicalSets(initial, symbols);
            ParsingTable parsingTable = GetParsingTable(C, symbols);

            Stack.Add(initial);

            string result = "";
            result += "digraph A {\r\n";
            result += "subgraph cluster_2 {\r\n";
            result += "label=\"Automaton\";\r\n";
            result += Dotify(C.First());
            result += "}\r\n";
            result += "subgraph cluster_3 {\r\n";
            result += "label=\"Parsing table\";\r\n";
            result += parsingTable.ToDot(C, symbols);
            result += "}\r\n";
            result += "}\r\n";

            Console.WriteLine(result);

            File.WriteAllText("output.txt", result);

            while (true)
            {
                ActionParsingTableEntry entry = parsingTable.FirstOrDefault(x => x is ActionParsingTableEntry a
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

        private ParsingTable GetParsingTable(List<ItemSet> C, List<ExpressionDefinition> symbols)
        {
            ParsingTable parsingTable = new ParsingTable();

            // Actions
            foreach (ItemSet set in C)
            {
                foreach (Item item in set)
                {
                    if (item.ExpressionAfterDot != null
                        && item.ExpressionAfterDot is TerminalExpressionDefinition ted)
                    {
                        parsingTable.Add(new ActionParsingTableEntry
                        {
                            ItemSet = set,
                            ExpressionDefinition = item.ExpressionAfterDot as TerminalExpressionDefinition,
                            ActionDescription = "s",
                            Action = Shift
                        });
                    }
                    else if (item.SubProduction.Production.Identifier == "Initial")
                    {
                        TerminalExpressionDefinition expressionDefinition = item.ExpressionAfterDot as TerminalExpressionDefinition;
                        expressionDefinition = expressionDefinition ?? new TerminalExpressionDefinition() { TokenType = TokenType.EndMarker };
                        parsingTable.Add(new ActionParsingTableEntry
                        {
                            ItemSet = set,
                            ExpressionDefinition = expressionDefinition,
                            ActionDescription = "a",
                            Action = Accept
                        });
                    }
                    else if (item.DotIndex == item.Count)
                    {
                        string identifier = item.SubProduction.Production.Identifier;
                        foreach (TerminalExpressionDefinition ted1 in new NonTerminalExpressionDefinition() { Identifier = identifier }.Follow())
                        {
                            parsingTable.Add(new ActionParsingTableEntry
                            {
                                ItemSet = set,
                                ExpressionDefinition = ted1,
                                ActionDescription = "r",
                                Action = (ItemSet arg1, ExpressionDefinition arg3) =>
                                    Reduce(arg1, arg3,item.SubProduction)
                            });
                        }
                    }
                }
            }

            // Goto's
            foreach (ItemSet set in C)
            {
                foreach (NonTerminalExpressionDefinition symbol in symbols.Where(x => x is NonTerminalExpressionDefinition))
                {
                    if (set.Transitions.ContainsKey(symbol))
                    {
                        parsingTable.Add(new GotoParsingTableEntry
                        {
                            ExpressionDefinition = symbol,
                            ItemSet = set,
                            Destination = set.Transitions[symbol]
                        });
                    }
                }
            }

            return parsingTable;
        }

        private static List<ItemSet> GetCanonicalSets(ItemSet initial, List<ExpressionDefinition> symbols)
        {
            List<ItemSet> C = new List<ItemSet>();
            C.Add(initial);
            while (true)
            {
                List<ItemSet> setsToAdd = new List<ItemSet>();
                foreach (ItemSet set in C)
                {
                    foreach (ExpressionDefinition symbol in symbols)
                    {
                        ItemSet _goto = set.Goto(symbol);

                        if (_goto.Count > 0)
                        {
                            ItemSet _existingGoto = C.FirstOrDefault(x => x.IsEqualTo(_goto));
                            if (_existingGoto == null)
                            {
                                setsToAdd.Add(_goto);
                                _existingGoto = _goto;
                            }

                            if (!set.Transitions.ContainsKey(symbol))
                            {
                                set.Transitions.Add(symbol, _existingGoto);
                            }
                        }
                    }
                }

                if (setsToAdd.Count == 0)
                {
                    break;
                }
                else
                {
                    C.AddRange(setsToAdd);
                }
            }

            return C;
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

            ItemSet tos = Stack.Last();
            for(int i = 0; i < subProduction.Count; i++)
            {
                Stack.RemoveAt(Stack.Count - 1);
                tos = Stack.Last();
                Symbols.RemoveAt(Symbols.Count - 1);
            }
            tos = Stack.Last();
            Stack.Add(tos.GetGoto(target));

            Symbols.Add(target);

            return true;
        }

        private bool Accept(ItemSet arg1, ExpressionDefinition arg3)
        {
            return false;
        }

        private bool Shift(ItemSet arg1, ExpressionDefinition arg3)
        {
            Stack.Add(arg1.Transitions[arg3]);
            Current = LexicalAnalyzer.GetNextToken();
            Symbols.Add(arg3);

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