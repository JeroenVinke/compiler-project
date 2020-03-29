﻿using Compiler.Common;
using Compiler.Parser.Common;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class RelOpASTNode : BooleanExpressionASTNode
    {
        public FactorASTNode Left { get; private set; }
        public FactorASTNode Right { get; private set; }
        public RelOp RelationOperator { get; private set; }
        public string RelOpAsString
        {
            get
            {
                if (RelationOperator == RelOp.Equals)
                {
                    return "==";
                }
                if (RelationOperator == RelOp.NotEquals)
                {
                    return "!=";
                }
                if (RelationOperator == RelOp.GreaterOrEqualThan)
                {
                    return ">=";
                }
                if (RelationOperator == RelOp.GreaterThan)
                {
                    return ">";
                }
                if (RelationOperator == RelOp.LessOrEqualThan)
                {
                    return "<=";
                }
                if (RelationOperator == RelOp.LessThan)
                {
                    return "<";
                }

                return "";
            }
        }

        public RelOpASTNode(FactorASTNode left, RelOp relationOperator, FactorASTNode right) : base(SyntaxTreeNodeType.RelOp)
        {
            Left = left;
            Right = right;
            RelationOperator = relationOperator;
        }

        public override string ToString()
        {
            return RelOpAsString;
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Address address1 = Left.GenerateCode(instructions);
            Address address2 = Right.GenerateCode(instructions);
            Address result = new Address();

            instructions.Add(new IfJumpInstruction(address1, RelOpAsString, address2, null));

            return result;
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Left, Right };
        }
    }
}
