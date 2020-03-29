using Compiler.Parser.Common;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public abstract class FactorASTNode : SyntaxTreeNode
    {
        public FactorASTNode(SyntaxTreeNodeType type) : base(type)
        {
        }
    }
}
