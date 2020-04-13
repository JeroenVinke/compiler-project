using Compiler.Common;
using System;
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
                    BooleanRule(),
                    AndRule(),
                    OrRule(),
                    EmptyRule(),
                }
            ));
        }

        private static SubProduction EmptyRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                    //new SemanticAction((ParsingNode node) => {
                    //    node.Attributes.Add("syntaxtreenode", node.GetAttribute<BooleanExpressionASTNode>("inh"));
                    //    node.Attributes.Add("syn", node.GetAttribute<BooleanExpressionASTNode>("inh"));
                    //})
                }
            );
        }

        private static SubProduction BooleanRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Boolean" }
                }
            );
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