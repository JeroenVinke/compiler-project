using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class IdentifierASTNode : FactorASTNode
    {
        public SymbolTableEntry SymbolTableEntry { get; set; }

        public IdentifierASTNode() : base(SyntaxTreeNodeType.Identifier)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            return SymbolTableEntry.Address;
        }

        public override string ToString()
        {
            return SymbolTableEntry.ToString();
        }
    }
}
