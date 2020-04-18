namespace Compiler.Parser
{
    public enum SyntaxTreeNodeType
    {
        Leaf,
        Plus,
        Min,
        Equals,
        Assignment,
        Statements,
        And,
        Or,
        BooleanExpression,
        While,
        Declaration,
        Boolean,
        NumericExpression,
        Identifier,
        Number,
        RelOp,
        If,
        IfElse
    }
}