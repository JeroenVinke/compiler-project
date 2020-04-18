using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class StatementRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production("Statement",
                new List<SubProduction>
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "OpenStatement" },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<SyntaxTreeNode>("OpenStatement", "syntaxtreenode");
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "ClosedStatement" },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<SyntaxTreeNode>("ClosedStatement", "syntaxtreenode");
                            })
                        }
                    )
                }
            ));

            grammar.Add(new Production("OpenStatement",
                new List<SubProduction>
                {
                    If(),
                    IfElse()
                }
            ));

            grammar.Add(new Production("ClosedStatement",
                new List<SubProduction>
                {
                    IfElseClosed(),
                    Declaration(),
                    Codeblock(),
                    Assignment(),
                    //While(),
                    //NumericExpression()
                }
            ));
        }

        private static SubProduction While()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.While },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = "Codeblock" },
                    //new SemanticAction((ParsingNode node) =>
                    //{
                    //    WhileASTNode syntaxTreeNode = new WhileASTNode();
                    //    syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                    //    syntaxTreeNode.Body = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    //    node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    //})
                }
            );
        }

        private static SubProduction If()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.If },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = "Statement" },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        IfASTNode syntaxTreeNode = new IfASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                        syntaxTreeNode.Body = node.GetAttributeForKey<SyntaxTreeNode>("Statement", "syntaxtreenode");
                        node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction IfElse()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.If },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = "ClosedStatement" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Else },
                    new NonTerminalExpressionDefinition { Identifier = "OpenStatement" },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        IfElseASTNode syntaxTreeNode = new IfElseASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                        syntaxTreeNode.IfBody = node.GetAttributeForKey<SyntaxTreeNode>("ClosedStatement", "syntaxtreenode");
                        syntaxTreeNode.ElseBody = node.GetAttributeForKey<SyntaxTreeNode>("OpenStatement", "syntaxtreenode");
                        node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction IfElseClosed()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.If },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Key = "ClosedStatement1", Identifier = "ClosedStatement" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Else },
                    new NonTerminalExpressionDefinition { Key = "ClosedStatement2", Identifier = "ClosedStatement" },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        IfElseASTNode syntaxTreeNode = new IfElseASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                        syntaxTreeNode.IfBody = node.GetAttributeForKey<SyntaxTreeNode>("ClosedStatement1", "syntaxtreenode");
                        syntaxTreeNode.ElseBody = node.GetAttributeForKey<SyntaxTreeNode>("ClosedStatement2", "syntaxtreenode");
                        node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction Assignment()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Identifier" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Assignment },
                    new NonTerminalExpressionDefinition { Identifier = "Factor" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        AssignmentASTNode syntaxTreeNode = new AssignmentASTNode();
                        syntaxTreeNode.SymbolTableEntry = node.GetAttributeForKey<SymbolTableEntry>("Identifier", "symboltableentry");
                        syntaxTreeNode.Value = node.GetAttributeForKey<NumericExpressionASTNode>("Factor", "syntaxtreenode");
                        node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction Codeblock()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Codeblock" },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    })
                }
            );
        }

        private static SubProduction Declaration()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Declaration" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<SyntaxTreeNode>("Declaration", "syntaxtreenode");
                    }),
                }
            );
        }
    }
}
