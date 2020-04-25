using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class CodeblockRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.Codeblock,
                new SubProduction
                (
                    new List<ExpressionDefinition>
                    {
                        new TerminalExpressionDefinition { TokenType = TokenType.BracketOpen },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            var symbolTableNode = node.FirstParentWithAttribute(ParserConstants.SymTable);

                            if (symbolTableNode == null)
                            {
                                node.Attributes[ParserConstants.SymTable] = node.Parser.RootSymbolTable;
                            }
                            else
                            {
                                node.Attributes[ParserConstants.SymTable] = symbolTableNode.GetAttribute<SymbolTable>(ParserConstants.SymTable).CreateChild();
                            }
                        }),
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.Statements },
                        new SemanticActionDefinition((ParsingNode node) => {
                            node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<StatementsASTNode>(ParserConstants.Statements, ParserConstants.SyntaxTreeNodes));
                        }),
                        new TerminalExpressionDefinition { TokenType = TokenType.BracketClose }
                    }
                )
            ));
        }
    }
}