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
            Func<ActionParsingTableEntry, bool> shift,
            Func<ActionParsingTableEntry, bool> accept,
            Func<ActionParsingTableEntry, bool> reduce)
        {
            ParsingTable parsingTable = new ParsingTable();

            // Actions
            foreach (ItemSet set in C)
            {
                if (set.Id == 18552)
                {
                    ;
                }

                foreach (Item item in set)
                {
                    if (item.ExpressionAfterDot != null
                        && item.ExpressionAfterDot is TerminalExpressionDefinition ted)
                    {
                        AddActionEntry(item, parsingTable, shift, "s", set, item.ExpressionAfterDot as TerminalExpressionDefinition);
                    }
                    else if (item.SubProduction.Production.Identifier == ParserConstants.Initial)
                    {
                        TerminalExpressionDefinition expressionDefinition = item.ExpressionAfterDot as TerminalExpressionDefinition;
                        expressionDefinition = expressionDefinition ?? new TerminalExpressionDefinition() { TokenType = TokenType.EndMarker };

                        AddActionEntry(item, parsingTable, accept, "a",  set, expressionDefinition);
                    }
                    else if (item.IsDotIndexAtEnd())
                    {
                        string identifier = item.SubProduction.Production.Identifier;
                        foreach (TerminalExpressionDefinition ted1 in new NonTerminalExpressionDefinition() { Identifier = identifier }.Follow())
                        {
                            AddActionEntry(item, parsingTable, reduce, "r", set, ted1);
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

        private static void AddActionEntry(Item item, ParsingTable parsingTable, Func<ActionParsingTableEntry, bool> action, string actionDescription, ItemSet set, TerminalExpressionDefinition expressionDefinition)
        {
            ActionParsingTableEntry existingEntry = (ActionParsingTableEntry)parsingTable.FirstOrDefault(x =>
                                        x is ActionParsingTableEntry apte
                                        && apte.ItemSet == set
                                        && apte.ActionDescription == actionDescription
                                        && apte.ExpressionDefinition.IsEqualTo(expressionDefinition));
            if (existingEntry == null)
            {
                parsingTable.Add(new ActionParsingTableEntry
                {
                    ItemSet = set,
                    ExpressionDefinition = expressionDefinition,
                    Items = new List<Item> { item },
                    ActionDescription = actionDescription,
                    Action = action
                });
            }
            else
            {
                existingEntry.Items.Add(item);
            }
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
