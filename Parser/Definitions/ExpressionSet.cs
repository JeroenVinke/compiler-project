using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class ExpressionSet : HashSet<TerminalExpressionDefinition>
    {
        public ExpressionSet()
        {
        }

        public ExpressionSet(IEnumerable<TerminalExpressionDefinition> collection) : base(collection)
        {
        }

        public override string ToString()
        {
            return "{" + string.Join(", ", this.Select(x => x.ToString()).ToList()) + "}";
        }

        internal void AddRangeUnique(IEnumerable<TerminalExpressionDefinition> first)
        {
            foreach(TerminalExpressionDefinition ted in first)
            {
                Add(ted);
            }
        }
    }
}