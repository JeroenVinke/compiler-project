using Compiler.Parser.Common;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class WhileASTNode : StatementASTNode
    {
        public BooleanExpressionASTNode Condition { get; set; }
        public StatementsASTNode Body { get; set; }

        public WhileASTNode() : base(SyntaxTreeNodeType.While)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Label conditionLabel = new Label();
            Label nextLabel = new Label();

            instructions.Add(conditionLabel);
            Address condition = Condition.GenerateCode(instructions);
            if (instructions.Last() is IfJumpInstruction j)
            {
                j.Label = nextLabel;
            }
            Body.GenerateCode(instructions);
            instructions.Add(new JumpInstruction(conditionLabel));

            instructions.Add(nextLabel);
            
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
