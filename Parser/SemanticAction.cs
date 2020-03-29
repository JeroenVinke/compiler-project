using Compiler.Parser;
using System;

namespace Compiler.Parser
{
    public class SemanticAction : ExpressionDefinition
    {
        public Action<ParsingNode> Action { get; set; }

        public SemanticAction(Action<ParsingNode> action)
        {
            Action = action;
        }

        public override ExpressionSet First()
        {
            return null;
        }
    }
}