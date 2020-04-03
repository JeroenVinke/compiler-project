﻿using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class StatementsRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(Statements());
            grammar.Add(Statements1());
        }

        private static Production Statements()
        {
            return new Production("Statements",
                new SubProduction
                (
                    new List<ExpressionDefinition>
                    {
                        new NonTerminalExpressionDefinition { Identifier = "Statement" },
                        new SemanticAction((ParsingNode node) =>
                        {
                        }),
                        new NonTerminalExpressionDefinition { Identifier = "Statements`" },
                        new SemanticAction((ParsingNode node) =>
                        {
                            StatementsASTNode astNode = new StatementsASTNode();
                            astNode.Statements.Add(node.GetAttributeForKey<StatementASTNode>("Statement", "syntaxtreenode"));
                            astNode.Statements.AddRange(node.GetAttributeForKey<List<StatementASTNode>>("Statements`", "syntaxtreenodes"));
                            node.Attributes.Add("syntaxtreenode", astNode);
                        })
                    }
                )
            );
        }

        private static Production Statements1()
        {
            return new Production("Statements`",
                new List<SubProduction>
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "Statement" },
                            new NonTerminalExpressionDefinition { Identifier = "Statements`" },
                            new SemanticAction((ParsingNode node) =>
                            {
                                List<StatementASTNode> syntaxTreeNodes = new List<StatementASTNode>();
                                syntaxTreeNodes.Add(node.GetAttributeForKey<StatementASTNode>("Statement", "syntaxtreenode"));
                                syntaxTreeNodes.AddRange(node.GetAttributeForKey<List<StatementASTNode>>("Statements`", "syntaxtreenodes"));

                                node.Attributes.Add("syntaxtreenodes", syntaxTreeNodes);
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            new SemanticAction((ParsingNode node) => {
                                List<StatementASTNode> syntaxTreeNodes = new List<StatementASTNode>();
                                node.Attributes.Add("syntaxtreenodes", syntaxTreeNodes);
                            })
                        }
                    )
                }
            );
        }
    }
}