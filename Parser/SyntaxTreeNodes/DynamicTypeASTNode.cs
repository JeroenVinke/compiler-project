using Compiler.Common;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class DynamicTypeASTNode : TypeASTNode
    {
        public SymbolTableEntry SymbolTableEntry { get; set; }

        public override string ToString()
        {
            return "Type";
        }
    }
}
