﻿using Compiler.Parser.Common;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class IfElseASTNode : StatementASTNode
    {
        public BooleanExpressionASTNode Condition { get; set; }
        public SyntaxTreeNode IfBody { get; set; }
        public SyntaxTreeNode ElseBody { get; set; }

        public IfElseASTNode() : base(SyntaxTreeNodeType.IfElse)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Condition.GenerateCode(instructions);

            Label trueLabel = new Label();
            instructions.Add(trueLabel);
            IfBody.GenerateCode(instructions);

            JumpInstruction doneJump = new JumpInstruction(null);
            NextInstructionsToBackpatch.Add(doneJump);
            instructions.Add(doneJump);

            Label falseLabel = new Label();
            instructions.Add(falseLabel);
            ElseBody.GenerateCode(instructions);
            ElseBody.NextInstructionsToBackpatch.AddRange(NextInstructionsToBackpatch);

            Condition.Backpatch(trueLabel, falseLabel);

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "IfElse";
        }

        public override bool Backpatch(Label nextLabel)
        {
            return base.Backpatch(nextLabel) | ElseBody.Backpatch(nextLabel);
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Condition, IfBody, ElseBody };
        }
    }
}
