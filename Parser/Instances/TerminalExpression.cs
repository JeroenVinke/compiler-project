using Compiler.Common;
using System;

namespace Compiler.Parser
{
    public class TerminalExpression : Expression
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

        public override string ToString()
        {
            return TokenType.GetName(typeof(TokenType), TokenType).ToString();
        }
    }
}