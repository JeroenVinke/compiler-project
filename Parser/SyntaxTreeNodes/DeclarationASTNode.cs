using Compiler.Common;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class DeclarationASTNode : StatementASTNode
    {
        public SymbolTableEntry SymbolTableEntry { get; set; }

        public DeclarationASTNode() : base(SyntaxTreeNodeType.Declaration)
        {
        }

        public override string ToString()
        {
            return "Declaration: " + SymbolTableEntry.ToString();
        }
    }
}
