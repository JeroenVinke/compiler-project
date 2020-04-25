using Compiler.Parser.Common;
using System.Collections.Generic;
using System.Linq;
using Compiler.Parser.Instructions;

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
            Condition.GenerateCode(instructions);

            Label trueLabel = new Label();
            instructions.Add(new LabelInstruction(trueLabel));
            Body.GenerateCode(instructions);
            instructions.Add(new JumpInstruction(trueLabel));

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
