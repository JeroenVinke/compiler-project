using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class DeclarationRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.Declaration,
                new SubProduction
                (
                    new List<ExpressionDefinition>
                    {
                        new TerminalExpressionDefinition { TokenType = TokenType.TypeDeclaration  },
                        new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            string type = node.GetAttributeForKey<WordToken>(ParserConstants.TypeDeclaration, ParserConstants.Token).Lexeme;

                            SymbolTable symbolTable = node.FirstParentWithAttribute(ParserConstants.SymTable).GetAttribute<SymbolTable>(ParserConstants.SymTable);
                            string key = node.GetAttributeForKey<WordToken>("Identifier", ParserConstants.Token).Lexeme;
                            SymbolTableEntryType symbolEntryType = SymbolTable.StringToSymbolTableEntryType(type);
                            SymbolTableEntry entry = symbolTable.Create(key, symbolEntryType);

                            DeclarationASTNode syntaxTreeNode = new DeclarationASTNode();
                            syntaxTreeNode.SymbolTableEntry = entry;
                            node.Attributes.Add(ParserConstants.SyntaxTreeNode, syntaxTreeNode);
                        })
                    }
                )
            ));;
        }
    }
}