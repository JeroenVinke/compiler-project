using System;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public abstract class Expression
    {
        public string Key { get; set; }
        public ParsingNode Tree { get; set; }

        internal static Expression Create(ExpressionDefinition x)
        {
            if (x is NonTerminalExpressionDefinition nted)
            {
                return new NonTerminalExpression { Identifier = nted.Identifier, Key = nted.Key };
            }
            if (x is TerminalExpressionDefinition ted)
            {
                return new TerminalExpression { TokenType = ted.TokenType, Key = ted.Key };
            }

            return null;
        }
    }
}