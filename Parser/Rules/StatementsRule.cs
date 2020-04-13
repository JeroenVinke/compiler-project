using Compiler.Common;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class StatementsRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(Statements());
        }

        private static Production Statements()
        {
            return new Production("Statements",
                new List<SubProduction>
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "Statements" },
                            new NonTerminalExpressionDefinition { Identifier = "Statement" },
                            //new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
                            //new SemanticActionDefinition((ParsingNode node) =>
                            //{
                            //}),
                            //new NonTerminalExpressionDefinition { Identifier = "Statements`" },
                            //new SemanticActionDefinition((ParsingNode node) =>
                            //{
                                //StatementsASTNode astNode = new StatementsASTNode();
                                //astNode.Statements.Add(node.GetAttributeForKey<StatementASTNode>("Statement", "syntaxtreenode"));
                                //astNode.Statements.AddRange(node.GetAttributeForKey<List<StatementASTNode>>("Statements`", "syntaxtreenodes"));
                                //node.Attributes.Add("syntaxtreenode", astNode);
                            //})
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "Statement" },
                            //new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
                            //new SemanticActionDefinition((ParsingNode node) =>
                            //{
                            //}),
                            //new NonTerminalExpressionDefinition { Identifier = "Statements`" },
                            //new SemanticActionDefinition((ParsingNode node) =>
                            //{
                                //StatementsASTNode astNode = new StatementsASTNode();
                                //astNode.Statements.Add(node.GetAttributeForKey<StatementASTNode>("Statement", "syntaxtreenode"));
                                //astNode.Statements.AddRange(node.GetAttributeForKey<List<StatementASTNode>>("Statements`", "syntaxtreenodes"));
                                //node.Attributes.Add("syntaxtreenode", astNode);
                            //})
                        }
                    ),
                    //new SubProduction
                    //(
                    //    new List<ExpressionDefinition>
                    //    {
                    //        new NonTerminalExpressionDefinition { Identifier = "Statement" },
                    //        new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
                    //    }
                    //),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            //new SemanticActionDefinition((ParsingNode node) => {
                            //    List<StatementASTNode> syntaxTreeNodes = new List<StatementASTNode>();
                            //    node.Attributes.Add("syntaxtreenodes", syntaxTreeNodes);
                            //})
                        }
                    )
                }
            );
        }
    }
}
