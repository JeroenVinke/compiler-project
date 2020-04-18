using Compiler.Common;
using Compiler.Parser;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class CodeblockRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production("Codeblock",
                new SubProduction
                (
                    new List<ExpressionDefinition>
                    {
                        new TerminalExpressionDefinition { TokenType = TokenType.BracketOpen },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            var symbolTableNode = node.FirstParentWithAttribute("symtable");

                            if (symbolTableNode == null)
                            {
                                node.Attributes["symtable"] = node.Parser.RootSymbolTable;
                            }
                            else
                            {
                                node.Attributes["symtable"] = symbolTableNode.GetAttribute<SymbolTable>("symtable").CreateChild();
                            }
                        }),
                        new NonTerminalExpressionDefinition { Identifier = "Statements" },
                        new SemanticActionDefinition((ParsingNode node) => {
                            node.Attributes.Add("syntaxtreenode", node.GetAttributeForKey<StatementsASTNode>("Statements", "syntaxtreenodes"));
                        }),
                        new TerminalExpressionDefinition { TokenType = TokenType.BracketClose }
                    }
                )
            ));
        }
    }
}