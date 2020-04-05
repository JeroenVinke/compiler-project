using Compiler.Parser.Instances;

namespace Compiler.Parser
{
    public class GotoParsingTableEntry : ParsingTableEntry
    {
        public ItemSet Destination { get; set; }
        public NonTerminalExpressionDefinition ExpressionDefinition { get; set; }

        public override string ToString()
        {
            return $"GOTO {ItemSet.Id}, {ExpressionDefinition.Key}, {Destination.Id}";
        }

        internal override string ShortDescription()
        {
            return $"g{Destination.Id}";
        }
    }
}
