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
            grammar.Add(new Production(ParserConstants.NumericExpression, new List<SubProduction> {
                PlusRule(),
                TermRule()
            }));

            grammar.Add(new Production(ParserConstants.NumericTerm, new List<SubProduction> {
                // * and /
                NumericFactorRule()
            }));

            grammar.Add(new Production(ParserConstants.NumericFactor, new List<SubProduction> {
                IdentifierRule(),
                ParenthesisRule(),
                IntegerRule()
            }));
        }

        private static SubProduction IdentifierRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Identifier },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.Identifier, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
        }

        private static SubProduction NumericFactorRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.NumericFactor },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.NumericFactor, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
        }

        private static SubProduction TermRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.NumericTerm },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.NumericTerm, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
        }

        private static SubProduction IntegerRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Integer },
                    new SemanticActionDefinition((ParsingNode node) => {
                        int value = Convert.ToInt32(node.GetAttributeForKey<WordToken>("Integer", ParserConstants.Token).Lexeme);
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new NumberASTNode() { Value = value });
                    })
                }
            );
        }

        private static SubProduction ParenthesisRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.NumericExpression },
                    new SemanticActionDefinition((ParsingNode node) =>{
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<NumericExpressionASTNode>(ParserConstants.NumericExpression, ParserConstants.SyntaxTreeNode));
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose }
                }
            );
        }

        private static SubProduction PlusRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition> {
                    new NonTerminalExpressionDefinition { Key = "NumericExpression1", Identifier = ParserConstants.NumericExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.Plus },
                    new NonTerminalExpressionDefinition { Key = "NumericExpression2", Identifier = ParserConstants.NumericExpression },
                    new SemanticActionDefinition((ParsingNode node) => {
                        FactorASTNode left = node.GetAttributeForKey<FactorASTNode>("NumericExpression1", ParserConstants.SyntaxTreeNode);
                        FactorASTNode right = node.GetAttributeForKey<FactorASTNode>("NumericExpression2", ParserConstants.SyntaxTreeNode);
                        AdditionASTNode syntaxTreeNode = new AdditionASTNode(left, right);
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, syntaxTreeNode);
                    })
                }
            );
        }
    }
}