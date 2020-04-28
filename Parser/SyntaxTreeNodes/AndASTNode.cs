using System.Collections.Generic;
using Compiler.Common;
using Compiler.Common.Instructions;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class AndASTNode : BooleanExpressionASTNode
    {
        public BooleanExpressionASTNode Left { get; private set; }
        public BooleanExpressionASTNode Right { get; private set; }

        public AndASTNode(BooleanExpressionASTNode left, BooleanExpressionASTNode right) : base(SyntaxTreeNodeType.And)
        {
            Left = left;
            Right = right;
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Left.GenerateCode(instructions);
            FalseList.AddRange(Left.FalseList);

            Right.GenerateCode(instructions);
            TrueList.AddRange(Right.TrueList);
            FalseList.AddRange(Right.FalseList);

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "&&";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Left, Right };
        }
    }
}
