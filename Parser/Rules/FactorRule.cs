using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class FactorRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(Factor());
            grammar.Add(new Production("Identifier", IdentifierRule()));
        }

        private static Production Factor()
        {
            return new Production("Factor",
                new List<SubProduction>
                {
                    IdentifierRule(),
                    //IntegerRule(),
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
                        SymbolTable symbolTable = node.FirstParentWithAttribute("symtable").GetAttribute<SymbolTable>("symtable");
                        string key = node.GetAttributeForKey<WordToken>("Identifier", "token").Lexeme;

                        SymbolTableEntry entry = null;
                        if (symbolTable.Get(key, out entry))
                        {
                            node.Attributes.Add("symboltableentry", entry);
                        }
                        else
                        {
                            throw new Exception();
                        }

                        node.Attributes.Add("token", node.GetAttributeForKey<WordToken>("Identifier", "token"));
                        node.Attributes.Add("syntaxtreenode", new IdentifierASTNode() { SymbolTableEntry = entry });
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
                    new NonTerminalExpressionDefinition { Identifier = "NumericExpression" },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes.Add("syntaxtreenode", node.GetAttributeForKey<NumericExpressionASTNode>("NumericExpression", "syntaxtreenode"));
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
                    new NonTerminalExpressionDefinition { Identifier = "Factor" },
                    new SemanticActionDefinition((ParsingNode node) =>{
                        node.Attributes.Add("syntaxtreenode", node.GetAttributeForKey<NumericExpressionASTNode>("Factor", "syntaxtreenode"));
                        node.Attributes.Add("value", node.GetAttributeForKey<NumericExpressionASTNode>("Factor", "value"));
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose }
                }
            );
        }
    }
}