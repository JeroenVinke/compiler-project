using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class OrExpression : SubExpression
    {
        public OrExpression(List<SubExpression> subExpressions)
        {
            SubExpressions = subExpressions;
        }

        public override string ToString()
        {
            return "(" + string.Join("|", SubExpressions.Select(x => x.ToString()).ToList()) + ")";
        }
    }
}
