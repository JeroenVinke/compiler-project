using Compiler.Parser.Common;
using Compiler.Parser.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class AdditionASTNode : NumericExpressionASTNode
    {
        public FactorASTNode Left { get; private set; }
        public FactorASTNode Right { get; private set; }

        public AdditionASTNode(FactorASTNode left, FactorASTNode right) : base(SyntaxTreeNodeType.Plus)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return "+";
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Address address1 = Left.GenerateCode(instructions);
            Address address2 = Right.GenerateCode(instructions);
            Address result = new Address();
            instructions.Add(new AddInstruction(address1, address2, result));

            return result;
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Left, Right };
        }
    }
}
