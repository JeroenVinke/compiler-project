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
            return new Production(ParserConstants.Statements,
                new List<SubProduction>
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.Statement },
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.Statements },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                StatementsASTNode astNode = new StatementsASTNode();
                                astNode.Statements.Add(node.GetAttributeForKey<StatementASTNode>(ParserConstants.Statement, ParserConstants.SyntaxTreeNode));
                                astNode.Statements.AddRange(node.GetAttributeForKey<StatementsASTNode>(ParserConstants.Statements, ParserConstants.SyntaxTreeNodes).Statements);
                                node.Attributes.Add(ParserConstants.SyntaxTreeNodes, astNode);
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
                                node.Attributes.Add(ParserConstants.SyntaxTreeNodes, new StatementsASTNode());
                            })
                        }
                    )
                }
            );
        }
    }
}
