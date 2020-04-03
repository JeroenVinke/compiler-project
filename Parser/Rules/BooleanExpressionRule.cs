using Compiler.Common;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class BooleanExpressionRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production("BooleanExpression",
                new List<SubProduction>
                {
                    OrRule(),
                    AndRule()
                }
            ));
        }

        private static SubProduction OrRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Or },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" }
                }
            );
        }

        private static SubProduction AndRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.And },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" }
                }
            );
        }
    }
}