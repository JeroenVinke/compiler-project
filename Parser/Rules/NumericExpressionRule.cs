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
                NumberRule(),
                PlusRule()
            }));
        }

        private static SubProduction NumberRule()
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