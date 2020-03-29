using System;

namespace Compiler.Common
{
    public class SingleCharacterToken : Token
    {
        public override string ToString()
        {
            return "<" + Enum.GetName(typeof(TokenType), Type) + ", " + Character + ">";
        }
    }
}
