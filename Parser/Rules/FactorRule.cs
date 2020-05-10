using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class FactorRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.Factor,
                new List<SubProduction>
                {
                    IdentifierRule(),
                    FunctionCallRule(),
                    BooleanExpressionRule(),
                    NumExpressionRule()
                }
            ));
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

        private static SubProduction FunctionCallRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.FunctionCall },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.FunctionCall, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
        }

        private static SubProduction BooleanExpressionRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.BooleanExpression, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
        }

        private static SubProduction NumExpressionRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.NumericExpression },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<NumericExpressionASTNode>(ParserConstants.NumericExpression, ParserConstants.SyntaxTreeNode));
                    })
                }
            );
        }
    }
}