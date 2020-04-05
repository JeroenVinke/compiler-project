using Compiler.Parser.Instances;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class ParsingTable : List<ParsingTableEntry>
    {
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
