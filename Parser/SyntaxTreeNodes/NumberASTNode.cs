using System.Collections.Generic;
using Compiler.Common;
using Compiler.Common.Instructions;

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
            return new ConstantValue(Value);
        }

        public override string ToString()
        {
            return "" + Value;
        }
    }
}
