using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class AndExpression : SubExpression
    {
        public AndExpression(List<SubExpression> subExpressions)
        {
            SubExpressions = subExpressions;
        }

        public override string ToString()
        {
            return string.Join("", SubExpressions.Select(x => x.ToString()).ToList());
        }
    }
}
