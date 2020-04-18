using Compiler.Parser.Instances;
using System;

namespace Compiler.Parser
{
    public class ActionParsingTableEntry : ParsingTableEntry
    {
        public string ActionDescription { get; set; }
        public Func<ItemSet, ExpressionDefinition, bool> Action { get; set; }
        public TerminalExpressionDefinition ExpressionDefinition { get; set; }

        public override string ToString()
        {
            return $"ACTION {ItemSet.Id}, {ExpressionDefinition.Key}, {ActionDescription}";
        }

        internal override string ShortDescription()
        {
            return $"{ActionDescription}";
        }
    }
}
