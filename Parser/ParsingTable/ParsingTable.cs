using Compiler.Common;
using Compiler.Parser.Instances;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class ParsingTable : List<ParsingTableEntry>
    {
        public static ParsingTable Create(List<ItemSet> C,
            List<ExpressionDefinition> symbols,
            Func<ItemSet, ExpressionDefinition, bool> shift,
            Func<ItemSet, ExpressionDefinition, bool> accept,
            Func<ItemSet, ExpressionDefinition, SubProduction, bool> reduce)
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
                            Action = shift
                        });
                    }
                    else if (item.SubProduction.Production.Identifier == ParserConstants.Initial)
                    {
                        TerminalExpressionDefinition expressionDefinition = item.ExpressionAfterDot as TerminalExpressionDefinition;
                        expressionDefinition = expressionDefinition ?? new TerminalExpressionDefinition() { TokenType = TokenType.EndMarker };
                        parsingTable.Add(new ActionParsingTableEntry
                        {
                            ItemSet = set,
                            ExpressionDefinition = expressionDefinition,
                            ActionDescription = "a",
                            Action = accept
                        });
                    }
                    else if (item.IsDotIndexAtEnd())
                    {
                        string identifier = item.SubProduction.Production.Identifier;
                        if (identifier == "ClosedStatement")
                        {
                            ;
                        }
                        foreach (TerminalExpressionDefinition ted1 in new NonTerminalExpressionDefinition() { Identifier = identifier }.Follow())
                        {
                            parsingTable.Add(new ActionParsingTableEntry
                            {
                                ItemSet = set,
                                ExpressionDefinition = ted1,
                                ActionDescription = "r",
                                Action = (ItemSet arg1, ExpressionDefinition arg3) =>
                                    reduce(arg1, arg3, item.SubProduction)
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
                    if (set.Transitions.Any(x => x.Key.IsEqualTo(symbol)))
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

        public string ToDot(List<ItemSet> states, List<ExpressionDefinition> symbols)
        {
            string result = "1111 [shape=plaintext,label=<<table>";

            result += "<tr><td></td>";
            foreach (ExpressionDefinition symbol in symbols.Where(x => x is TerminalExpressionDefinition))
            {
                result += $"<td>{symbol.Key}</td>";
            }
            result += $"<td></td>";
            foreach (NonTerminalExpressionDefinition symbol in symbols.Where(x => x is NonTerminalExpressionDefinition))
            {
                result += $"<td>{symbol.Key}</td>";
            }
            result += "</tr>";

            foreach (ItemSet set in states)
            {
                string row = $"<tr><td>{set.Id}</td>";

                foreach (ExpressionDefinition definition in symbols.Where(x => x is TerminalExpressionDefinition))
                {
                    var action = this.FirstOrDefault(x => x is ActionParsingTableEntry  a
                        && a.ItemSet == set 
                        && a.ExpressionDefinition.IsEqualTo(definition));

                    if(action != null)
                    {
                        row += $"<td>{action.ShortDescription()}</td>";
                    }
                    else
                    {
                        row += $"<td></td>";
                    }
                }

                row += $"<td></td>";

                foreach (NonTerminalExpressionDefinition definition in symbols.Where(x => x is NonTerminalExpressionDefinition))
                {
                    var @goto = this.FirstOrDefault(x => x is GotoParsingTableEntry a
                        && a.ItemSet == set
                        && a.ExpressionDefinition.IsEqualTo(definition));

                    if (@goto != null)
                    {
                        row += $"<td>{@goto.ShortDescription()}</td>";
                    }
                    else
                    {
                        row += $"<td></td>";
                    }
                }

                row += "</tr>";
                result += row;
            }

            result += "</table>>]\r\n";

            return result;
        }
    }
}
