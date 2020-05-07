using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class TypeRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.Type,
                new List<SubProduction>
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                SymbolTable symbolTable = node.CurrentSymbolTable;

                                string name = node.GetAttributeForKey<WordToken>(ParserConstants.Identifier, ParserConstants.Token).Lexeme;

                                if(symbolTable.Get(name, out SymbolTableEntry entry))
                                {
                                    node.Attributes.Add(ParserConstants.SyntaxTreeNode, new DynamicTypeASTNode()
                                    {
                                        SymbolTableEntry = entry
                                    });
                                }
                                else
                                {
                                    throw new System.Exception($"Entry for '{name}' not found in the symboltable");
                                }
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.TypeDeclaration },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                string token = node.GetAttributeForKey<WordToken>(ParserConstants.TypeDeclaration, ParserConstants.Token).Lexeme;

                                node.Attributes.Add(ParserConstants.SyntaxTreeNode, new StaticTypeASTNode()
                                {
                                    SymbolTableEntryType = SymbolTable.StringToSymbolTableEntryType(token)
                                });
                            })
                        }
                    )
                }
            ));
        }
    }
}