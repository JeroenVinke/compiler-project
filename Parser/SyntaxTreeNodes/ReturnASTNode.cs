using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class ReturnASTNode : StatementASTNode
    {
        public SyntaxTreeNode ReturnValue { get; internal set; }

        public ReturnASTNode() : base(SyntaxTreeNodeType.Return)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Address returnValueAddress = ReturnValue.GenerateCode(instructions);
            instructions.Add(new ReturnInstruction(returnValueAddress));

            return base.GenerateCode(instructions);
        }
    }
}
