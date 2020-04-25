using Compiler.Parser.Common;
using System.Collections.Generic;
using Compiler.Parser.Instructions;
using System.Linq;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class IfASTNode : StatementASTNode
    {
        public BooleanExpressionASTNode Condition { get; set; }
        public SyntaxTreeNode Body { get; set; }

        public IfASTNode() : base(SyntaxTreeNodeType.If)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Condition.GenerateCode(instructions);

            Label trueLabel = new Label();
            instructions.Add(new LabelInstruction(trueLabel));
            Body.GenerateCode(instructions);

            Label falseLabel = new Label();
            instructions.Add(new LabelInstruction(falseLabel));

            Condition.Backpatch(trueLabel, falseLabel);

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "If";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Condition, Body };
        }
    }
}
