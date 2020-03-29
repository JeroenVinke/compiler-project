using Compiler.Common;
using Compiler.Common.Instructions;
using Compiler.Parser.Common;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class AssignmentASTNode : StatementASTNode
    {
        public SymbolTableEntry SymbolTableEntry { get; set; }
        public FactorASTNode Value { get; set; }

        public AssignmentASTNode() : base(SyntaxTreeNodeType.Assignment)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Address address = Value.GenerateCode(instructions);

            instructions.Add(new AssignmentInstruction(SymbolTableEntry.Address, address));

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return $"{SymbolTableEntry.ToString()} =";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Value };
        }
    }
}
