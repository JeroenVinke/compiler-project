using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class FactorRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(Factor());
            grammar.Add(new Production(ParserConstants.Identifier, IdentifierRule()));
        }

        private static Production Factor()
        {
            return new Production(ParserConstants.Factor,
                new List<SubProduction>
                {
                    IdentifierRule(),
                    ParenthesisRule(),
                    NumExpressionRule()
                }
            );
        }

        private static SubProduction IdentifierRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        SymbolTable symbolTable = node.CurrentSymbolTable;
                        string key = node.GetAttributeForKey<WordToken>(ParserConstants.Identifier, ParserConstants.Token).Lexeme;

                        SymbolTableEntry entry = symbolTable.GetOrThrow(key, out entry);

                        node.Attributes.Add(ParserConstants.SymbolTableEntry, entry);
                        node.Attributes.Add(ParserConstants.Token, node.GetAttributeForKey<WordToken>(ParserConstants.Identifier, ParserConstants.Token));
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new IdentifierASTNode() { SymbolTableEntry = entry });
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

        private static SubProduction ParenthesisRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factor },
                    new SemanticActionDefinition((ParsingNode node) =>{
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<NumericExpressionASTNode>(ParserConstants.Factor, ParserConstants.SyntaxTreeNode));
                        node.Attributes.Add("value", node.GetAttributeForKey<NumericExpressionASTNode>(ParserConstants.Factor, "value"));
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose }
                }
            );
        }
    }
}