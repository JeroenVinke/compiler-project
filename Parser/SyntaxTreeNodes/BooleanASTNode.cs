﻿using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class BooleanASTNode : BooleanExpressionASTNode
    {
        public bool Value { get; set; }

        public BooleanASTNode(bool value) : base(SyntaxTreeNodeType.Boolean)
        {
            Value = value;
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Address address = new Address();
            instructions.Add(new AssignmentInstruction(address, new ConstantValue(Value)));
            return address;
        }

        public override string ToString()
        {
            return Value ? "true" : "false";
        }
    }
}
