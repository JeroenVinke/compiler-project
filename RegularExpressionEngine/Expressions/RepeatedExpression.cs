namespace Compiler.RegularExpressionEngine
{
    internal class RepeatedExpression : SubExpression
    {
        public SubExpression Expression;
        public char RepetitionOperator;

        public RepeatedExpression(SubExpression expr, char v)
        {
            this.Expression = expr;
            this.RepetitionOperator = v;
        }

        public override string ToString()
        {
            return Expression.ToString() + RepetitionOperator;
        }
    }
}
