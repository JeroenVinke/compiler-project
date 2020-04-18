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
                    new SemanticActionDefinition((ParsingNode node) => {
                        node.Attributes.Add("syntaxtreenode", node.GetAttribute<BooleanExpressionASTNode>("inh"));
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
                    new NonTerminalExpressionDefinition { Identifier = "Boolean" },
                    new SemanticActionDefinition((ParsingNode node) => {
                        node.Attributes.Add("syntaxtreenode", node.GetAttributeForKey<SyntaxTreeNode>("Boolean", "syntaxtreenode"));
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
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Or },
                    new NonTerminalExpressionDefinition { Key = "BooleanExpression2", Identifier = "BooleanExpression" },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        BooleanExpressionASTNode left = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                        BooleanExpressionASTNode right = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression2", "syntaxtreenode");

                        OrASTNode syntaxTreeNode = new OrASTNode(left, right);
                        node.Attributes.Add("syntaxtreenode", syntaxTreeNode);
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
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.And },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        BooleanExpressionASTNode left = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                        BooleanExpressionASTNode right = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression2", "syntaxtreenode");

                        AndASTNode syntaxTreeNode = new AndASTNode(left, right);
                        node.Attributes.Add("syntaxtreenode", syntaxTreeNode);
                    })
                }
            );
        }
    }
}