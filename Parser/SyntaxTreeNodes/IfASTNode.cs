using Compiler.Parser.Common;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class IfASTNode : StatementASTNode
    {
        public BooleanExpressionASTNode Condition { get; set; }
        public StatementsASTNode Body { get; set; }

        public IfASTNode() : base(SyntaxTreeNodeType.While)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Condition.GenerateCode(instructions);

            Label trueLabel = new Label();
            instructions.Add(trueLabel);
            Body.GenerateCode(instructions);

            Label falseLabel = new Label();
            instructions.Add(falseLabel);

            Condition.Backpatch(trueLabel, falseLabel);

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "While";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Condition, Body };
        }
    }
}
