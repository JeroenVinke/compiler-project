namespace Compiler.Parser.SyntaxTreeNodes
{
    public abstract class BooleanExpressionASTNode : FactorASTNode
    {
        public BooleanExpressionASTNode(SyntaxTreeNodeType type) : base(type)
        {
        }
    }
}
