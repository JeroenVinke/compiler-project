using Compiler.Common;
using System;

namespace Compiler.Parser
{
    public class TerminalExpressionDefinition : ExpressionDefinition
    {
        private TokenType tokenType;

        public TokenType TokenType { 
            get => tokenType;
            set
            {
                tokenType = value;
                if(string.IsNullOrEmpty(Key))
                {
                    Key = Enum.GetName(typeof(TokenType), value);
                }
            }
        }

        public override ExpressionSet First()
        {
            return new ExpressionSet() { this };
        }

        public override string ToString()
        {
            return TokenType.GetName(typeof(TokenType), TokenType).ToString();
        }

        public override bool IsEqualTo(ExpressionDefinition definition)
        {
            if (definition is TerminalExpressionDefinition nte)
            {
                return TokenType == nte.TokenType;
            }

            return false;
        }
    }
}