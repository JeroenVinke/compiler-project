using System.Collections.Generic;

namespace Compiler.Parser.Instances
{
    public class Item : List<Expression>
    {
        public SubProduction SubProduction { get; set; }

        private Expression _expressionAfterDot;
        public Expression ExpressionAfterDot
        { 
            get
            {
                if (_expressionAfterDot == null)
                {
                    _expressionAfterDot = this[0];
                }

                return _expressionAfterDot;
            }
            set
            {
                _expressionAfterDot = value;
            }
        }

        public Item(List<Expression> expressions) : base(expressions)
        {
        }

        public Item(SubProduction subProduction)
        {
            SubProduction = subProduction;

            foreach(ExpressionDefinition expressionDefinition in subProduction)
            {
                if (expressionDefinition is NonTerminalExpressionDefinition nte)
                {
                    Add(new NonTerminalExpression
                    {
                        Key = nte.Key,
                        Identifier = nte.Identifier
                    });
                }
                else if (expressionDefinition is TerminalExpressionDefinition te)
                {
                    Add(new TerminalExpression
                    {
                        Key = te.Key,
                        TokenType = te.TokenType
                    });
                }
            }
        }

        public void Next()
        {
            int nextIndex = IndexOf(ExpressionAfterDot) + 1;

            if (Count > nextIndex + 1)
            {
                ExpressionAfterDot = this[nextIndex];
            }
        }

        public Item Clone()
        {
            Item item = new Item(SubProduction);
            item.ExpressionAfterDot = ExpressionAfterDot;
            return item;
        }
    }
}
