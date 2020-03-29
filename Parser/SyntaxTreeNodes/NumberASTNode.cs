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
            Address address = new Address();
            instructions.Add(new AssignmentInstruction(address, Value));
            return address;
        }

        public override string ToString()
        {
            return "" + Value;
        }
    }
}
