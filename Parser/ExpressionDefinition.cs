using System.Collections.Generic;

namespace Compiler.Parser
{
    public abstract class ExpressionDefinition
    {
        public abstract ExpressionSet First();
        public SubProduction SubProduction { get; set; }
        public string Key { get; set; }
    }
}