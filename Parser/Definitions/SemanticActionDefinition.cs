using Compiler.Parser;
using System;

namespace Compiler.Parser
{
    public class SemanticActionDefinition : ExpressionDefinition
    {
        public Action<ParsingNode> Action { get; set; }

        public SemanticActionDefinition(Action<ParsingNode> action)
        {
            Action = action;
        }

        public override ExpressionSet First()
        {
            return null;
        }
    }
}