using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;
using System.Reflection.Emit;

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
                            new NonTerminalExpressionDefinition { Identifier = "Statement``" },
                            new SemanticAction((ParsingNode node) =>
                            {
                                node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementASTNode>("Statement``", "syntaxtreenode");
                            })
                        }
                    )
                }
            ));

            grammar.Add(new Production("Statement``",
                new List<SubProduction>
                {
                    Declaration(),
                    Codeblock(),
                    Assignment(),
                    While()
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
                    new SemanticAction((ParsingNode node) =>
                    {
                        WhileASTNode syntaxTreeNode = new WhileASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                        syntaxTreeNode.Body = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
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
                    new NonTerminalExpressionDefinition { Identifier = "NumericExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new SemanticAction((ParsingNode node) =>
                    {
                        AssignmentASTNode syntaxTreeNode = new AssignmentASTNode();
                        syntaxTreeNode.SymbolTableEntry = node.GetAttributeForKey<SymbolTableEntry>("Identifier", "symboltableentry");
                        syntaxTreeNode.Value = node.GetAttributeForKey<NumericExpressionASTNode>("NumericExpression", "syntaxtreenode");
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
                    new SemanticAction((ParsingNode node) =>
                    {
                        node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
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
                    new SemanticAction((ParsingNode node) =>
                    {
                        node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<SyntaxTreeNode>("Declaration", "syntaxtreenode");
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
                }
            );
        }
    }
}
