using System.Collections.Generic;
using Compiler.Common;
using Compiler.Common.Instructions;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class WhileASTNode : StatementASTNode
    {
        public BooleanExpressionASTNode Condition { get; set; }
        public SyntaxTreeNode Body { get; set; }

        public WhileASTNode() : base(SyntaxTreeNodeType.While)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Label startLabel = new Label();
            instructions.Add(new LabelInstruction(startLabel));
            Condition.GenerateCode(instructions);

            Label trueLabel = new Label();
            instructions.Add(new LabelInstruction(trueLabel));
            Body.GenerateCode(instructions);
            instructions.Add(new JumpInstruction(startLabel));

            Label falseLabel = new Label();
            instructions.Add(new LabelInstruction(falseLabel));

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
