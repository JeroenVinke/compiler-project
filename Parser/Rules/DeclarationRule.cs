using Compiler.Common;
using Compiler.Parser;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class DeclarationRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production("Declaration",
                new SubProduction
                (
                    new List<ExpressionDefinition>
                    {
                        new TerminalExpressionDefinition { TokenType = TokenType.TypeDeclaration  },
                        new TerminalExpressionDefinition { TokenType = TokenType.Identifier }
                        //new SemanticAction((ParsingNode node) =>
                        //{
                        //    string type = node.GetAttributeForKey<WordToken>("TypeDeclaration", "token").Lexeme;

                        //    SymbolTable symbolTable = node.FirstParentWithAttribute("symtable").GetAttribute<SymbolTable>("symtable");
                        //    string key = node.GetAttributeForKey<WordToken>("Identifier", "token").Lexeme;
                        //    SymbolTableEntryType symbolEntryType = SymbolTable.StringToSymbolTableEntryType(type);
                        //    SymbolTableEntry entry = symbolTable.Create(key, symbolEntryType);
                        //})
                    }
                )
            ));;
        }
    }
}