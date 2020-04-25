namespace Compiler.Parser.SyntaxTreeNodes
{
    public class TypeASTNode : SyntaxTreeNode
    {
        public TypeASTNode() : base(SyntaxTreeNodeType.Type)
        {
        }

        public override string ToString()
        {
            return "Type";
        }
    }
}
