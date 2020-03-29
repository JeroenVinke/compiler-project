using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser
{
    public class SubProduction : List<ExpressionDefinition>
    {
        public SubProduction(IEnumerable<ExpressionDefinition> collection) : base(collection)
        {
            ForEach(x => x.SubProduction = this);
        }

        public Production Production { get; internal set; }

        public override string ToString()
        {
            return string.Join(" ", this.Select(y => y.ToString()));
        }
    }
}
