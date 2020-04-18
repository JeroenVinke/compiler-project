using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
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
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                StatementsASTNode astNode = new StatementsASTNode();
                                astNode.Statements.AddRange(node.GetAttributeForKey<StatementsASTNode>("Statements", "syntaxtreenodes").Statements);
                                astNode.Statements.Add(node.GetAttributeForKey<StatementASTNode>("Statement", "syntaxtreenode"));
                                node.Attributes.Add("syntaxtreenodes", astNode);
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "Statement" },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                StatementsASTNode astNode = new StatementsASTNode();
                                astNode.Statements.Add(node.GetAttributeForKey<StatementASTNode>("Statement", "syntaxtreenode"));
                                node.Attributes.Add("syntaxtreenodes", astNode);
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes.Add("syntaxtreenodes", new StatementsASTNode());
                            })
                        }
                    )
                }
            );
        }
    }
}
