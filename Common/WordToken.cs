using System;

namespace Compiler.Common
{
    public class WordToken : Token
    {
        public string Lexeme { get; set; }

        public override string ToString()
        {
            return "<" + Enum.GetName(typeof(TokenType), Type) + ", " + Lexeme + ">";
        }
    }
}
