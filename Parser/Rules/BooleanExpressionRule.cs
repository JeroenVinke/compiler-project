using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class BooleanExpressionRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.BooleanExpression,
                new List<SubProduction>
                {
                    AndRule(),
                    OrRule(),
                    TermRule()
                }
            ));

            grammar.Add(new Production(ParserConstants.BooleanTerm,
                new List<SubProduction>
                {
                    ParenthesisRule(),
                    IdentifierRule(),
                    BooleanRule(),
                    RelopRules()
                }
            ));
        }
        private static SubProduction RelopRules()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.NumericExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.RelOp },
                    new NonTerminalExpressionDefinition { Key = "Factor2", Identifier = ParserConstants.NumericExpression },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string relOpToken = node.GetAttributeForKey<WordToken>("RelOp", ParserConstants.Token).Lexeme;
                        RelOp relOp;

                        if (relOpToken == "==")
                        {
                            relOp = RelOp.Equals;
                        }
                        else if (relOpToken == "!=")
                        {
                            relOp = RelOp.NotEquals;
                        }
                        else if (relOpToken == "<")
                        {
                            relOp = RelOp.LessThan;
                        }
                        else if (relOpToken == "<=")
                        {
                            relOp = RelOp.LessOrEqualThan;
                        }
                        else if (relOpToken == ">")
                        {
                            relOp = RelOp.GreaterThan;
                        }
                        else if (relOpToken == ">=")
                        {
                            relOp = RelOp.GreaterOrEqualThan;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        FactorASTNode left = node.GetAttributeForKey<FactorASTNode>(ParserConstants.NumericExpression, ParserConstants.SyntaxTreeNode);
                        FactorASTNode right = node.GetAttributeForKey<FactorASTNode>("Factor2", ParserConstants.SyntaxTreeNode);
                        RelOpASTNode syntaxTreeNode = new RelOpASTNode(left, relOp, right);

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, syntaxTreeNode);
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
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanTerm },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.BooleanTerm, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
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

        private static SubProduction EmptyRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                    new SemanticActionDefinition((ParsingNode node) => {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttribute<BooleanExpressionASTNode>("inh"));
                    })
                }
            );
        }

        private static SubProduction BooleanRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Boolean },
                    new SemanticActionDefinition((ParsingNode node) => {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.Boolean, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
        }

        private static SubProduction OrRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.Or },
                    new NonTerminalExpressionDefinition { Key = "BooleanExpression2", Identifier = ParserConstants.BooleanExpression },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        BooleanExpressionASTNode left = node.GetAttributeForKey<BooleanExpressionASTNode>(ParserConstants.BooleanExpression, ParserConstants.SyntaxTreeNode);
                        BooleanExpressionASTNode right = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression2", ParserConstants.SyntaxTreeNode);

                        OrASTNode syntaxTreeNode = new OrASTNode(left, right);
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, syntaxTreeNode);
                    })
                }
            );
        }

        private static SubProduction AndRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.And },
                    new NonTerminalExpressionDefinition { Key = "BooleanExpression2", Identifier = ParserConstants.BooleanExpression },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        BooleanExpressionASTNode left = node.GetAttributeForKey<BooleanExpressionASTNode>(ParserConstants.BooleanExpression, ParserConstants.SyntaxTreeNode);
                        BooleanExpressionASTNode right = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression2", ParserConstants.SyntaxTreeNode);

                        AndASTNode syntaxTreeNode = new AndASTNode(left, right);
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, syntaxTreeNode);
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
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new SemanticActionDefinition((ParsingNode node) =>{
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.BooleanExpression, ParserConstants.SyntaxTreeNode));
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose }
                }
            );
        }
    }
}