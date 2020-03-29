using Compiler.Common.Instructions;
using Compiler.Parser.Common;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class NumberASTNode : NumericExpressionASTNode
    {
        public int Value { get; set; }

        public NumberASTNode() : base(SyntaxTreeNodeType.Number)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            return new Address("", "" + Value);
        }

        public override string ToString()
        {
            return "" + Value;
        }
    }
}
