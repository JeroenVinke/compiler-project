using System.Collections.Generic;

namespace Compiler.RegularExpressionEngine
{
    public class SubExpression
    {
        public List<SubExpression> SubExpressions { get; set; } = new List<SubExpression>();
    }
}
