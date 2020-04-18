using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class NumericExpressionRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production("NumericExpression", new List<SubProduction> {
                NumberRule(),
                PlusRule(),
                EmptyRule()
            }));
        }

        private static SubProduction EmptyRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                    new SemanticActionDefinition((ParsingNode node) => {
                        node.Attributes.Add("syntaxtreenode", node.GetAttribute<BooleanExpressionASTNode>("inh"));
                        node.Attributes.Add("syn", node.GetAttribute<BooleanExpressionASTNode>("inh"));
                    })
                }
            );
        }

        private static SubProduction NumberRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Integer },
                    new SemanticActionDefinition((ParsingNode node) => {
                        int value = Convert.ToInt32(node.GetAttributeForKey<WordToken>("Integer", "token").Lexeme);
                        node.Attributes.Add("syntaxtreenode", new NumberASTNode() { Value = value });
                    })
                }
            );
        }

        private static SubProduction PlusRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition> {
                    new NonTerminalExpressionDefinition { Key = "NumericExpression1", Identifier = "NumericExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Plus },
                    new NonTerminalExpressionDefinition { Key = "NumericExpression2", Identifier = "NumericExpression" },
                    new SemanticActionDefinition((ParsingNode node) => {
                        FactorASTNode left = node.GetAttributeForKey<FactorASTNode>("NumericExpression1", "syntaxtreenode");
                        FactorASTNode right = node.GetAttributeForKey<FactorASTNode>("NumericExpression2", "syntaxtreenode");
                        AdditionASTNode syntaxTreeNode = new AdditionASTNode(left, right);
                        node.Attributes.Add("syntaxtreenode", syntaxTreeNode);
                    })
                }
            );
        }
    }
}