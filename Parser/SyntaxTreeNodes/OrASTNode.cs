using Compiler.Common.Instructions;
using Compiler.Parser.Common;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class OrASTNode : BooleanExpressionASTNode
    {
        public BooleanExpressionASTNode Left { get; private set; }
        public BooleanExpressionASTNode Right { get; private set; }

        public OrASTNode(BooleanExpressionASTNode left, BooleanExpressionASTNode right) : base(SyntaxTreeNodeType.Or)
        {
            Left = left;
            Right = right;
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Address address = Left.GenerateCode(instructions);
            Label orLabel = new Label();

            Address f = new Address();
            instructions.Add(new AssignmentInstruction(f, false));
            instructions.Add(new IfJumpInstruction(address, "!=", f, orLabel));

            instructions.Add(orLabel);
            Right.GenerateCode(instructions);

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "||";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Left, Right };
        }
    }
}
