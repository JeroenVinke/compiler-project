using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public static class FunctionCallRule
    {
        public const string Factors = "Factors";

        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.FunctionCall, FunctionCall()));
        }

        private static SubProduction FunctionCall()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    //new NonTerminalExpressionDefinition { Identifier = ParserConstants.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        SymbolTable symbolTable = node.CurrentSymbolTable;
                        string key = node.GetAttributeForKey<WordToken>(ParserConstants.Identifier, ParserConstants.Token).Lexeme;

                        SymbolTableEntry entry = symbolTable.GetOrThrow(key, out entry);

                        node.GetNodeForKey("Identifier").Attributes.Add(ParserConstants.SymbolTableEntry, entry);
                        node.GetNodeForKey("Identifier").Attributes.Add(ParserConstants.SyntaxTreeNode, new IdentifierASTNode() { SymbolTableEntry = entry });
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factors },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        FunctionCallASTNode syntaxTreeNode = new FunctionCallASTNode();
                        syntaxTreeNode.Target = node.GetAttributeForKey<SymbolTableEntry>(ParserConstants.Identifier, ParserConstants.SymbolTableEntry);
                        syntaxTreeNode.Arguments = node.GetAttributeForKey<List<FactorASTNode>>(ParserConstants.Factors, Factors);

                        FunctionASTNode functionASTNode = syntaxTreeNode.Target.GetMetadata<FunctionASTNode>("FunctionASTNode");
                        syntaxTreeNode.FunctionASTNode = functionASTNode;



                        node.Attributes[ParserConstants.SyntaxTreeNode] = syntaxTreeNode;
                    })
                }
            );
        }
    }
}
