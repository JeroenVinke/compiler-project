using System.Collections.Generic;
using Compiler.Common;
using Compiler.Common.Instructions;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public abstract class BooleanExpressionASTNode : FactorASTNode
    {
        public List<JumpInstruction> TrueList { get; set; } = new List<JumpInstruction>();
        public List<JumpInstruction> FalseList { get; set; } = new List<JumpInstruction>();

        public BooleanExpressionASTNode(SyntaxTreeNodeType type) : base(type)
        {
        }

        public void Backpatch(Label trueLabel, Label falseLabel)
        {
            foreach(JumpInstruction instruction in TrueList)
            {
                instruction.Label = trueLabel;
            }

            foreach (JumpInstruction instruction in FalseList)
            {
                instruction.Label = falseLabel;
            }
        }
    }
}
