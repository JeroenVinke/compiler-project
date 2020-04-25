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
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
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
    }
}