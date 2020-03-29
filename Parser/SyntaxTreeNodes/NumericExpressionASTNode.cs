namespace Compiler.Parser.SyntaxTreeNodes
{
    public abstract class NumericExpressionASTNode : FactorASTNode
    {
        public NumericExpressionASTNode(SyntaxTreeNodeType type) : base(type)
        {
        }
    }
}
