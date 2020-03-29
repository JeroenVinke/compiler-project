using Compiler.Common;
using System;

namespace Compiler.Parser
{
    public class ParsingTableEntry
    {
        public string Dimension1 { get; set; }
        public TerminalExpressionDefinition Dimension2 { get; set; }
        public SubProduction SubProduction { get; set; }

        public override string ToString()
        {
            return Dimension1 + " : " + Enum.GetName(typeof(TokenType), Dimension2.TokenType) + " -> " + SubProduction.ToString();
        }
    }
}