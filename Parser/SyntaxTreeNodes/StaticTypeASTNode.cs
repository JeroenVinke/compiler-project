using Compiler.Common;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class StaticTypeASTNode : TypeASTNode
    {
        public SymbolTableEntryType SymbolTableEntryType { get; set; }

        public override string ToString()
        {
            return "Type";
        }
    }
}
