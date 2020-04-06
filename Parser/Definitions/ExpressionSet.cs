using Compiler.Common;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class ExpressionSet : List<TerminalExpressionDefinition>
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

        internal void AddRangeUnique(ExpressionSet first)
        {
            AddRange(first.Where(x => !this.Any(y => y.TokenType == x.TokenType)).ToList());
        }
    }
}