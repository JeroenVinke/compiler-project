using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class ReturnASTNode : StatementASTNode
    {
        public ReturnASTNode() : base(SyntaxTreeNodeType.Return)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            instructions.Add(new ReturnInstruction());

            return base.GenerateCode(instructions);
        }
    }
}
