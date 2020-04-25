using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class FunctionRule
    {
        private const string Entry = "Entry";
        private const string Entries = "Entries";

        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.ClassMembers,
                new List<SubProduction>
                {
                    new SubProduction(new List<ExpressionDefinition>()
                    {
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.ClassMembers },
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.ClassMember },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            List<SyntaxTreeNode> nodes = new List<SyntaxTreeNode>();
                            nodes.AddRange(node.GetAttributeForKey<List<SyntaxTreeNode>>(ParserConstants.ClassMembers, ParserConstants.SyntaxTreeNodes));
                            nodes.Add(node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.ClassMember, ParserConstants.SyntaxTreeNode));

                            node.Attributes.Add(ParserConstants.SyntaxTreeNodes, nodes);
                        })
                    }),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes.Add(ParserConstants.SyntaxTreeNodes, new List<SyntaxTreeNode> {});
                            })
                        }
                    )
                }
            ));

            grammar.Add(new Production(ParserConstants.ClassMember,
                new SubProduction
                (
                    new List<ExpressionDefinition>
                    {
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.Function },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            node.Attributes.Add(ParserConstants.SyntaxTreeNode, node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.Function, ParserConstants.SyntaxTreeNode));
                        })
                    }
                )
            ));

            grammar.Add(new Production(ParserConstants.Function,
                new SubProduction
                (
                    new List<ExpressionDefinition>
                    {
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.Type },
                        new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            SymbolTable symbolTable = node.CurrentSymbolTable;

                            string key = node.GetAttributeForKey<WordToken>(ParserConstants.Identifier, ParserConstants.Token).Lexeme;
                            SymbolTableEntry entry = symbolTable.Create(key, SymbolTableEntryType.Function);

                            symbolTable = symbolTable.CreateChild();

                            node.Attributes[ParserConstants.SymTable] = symbolTable;

                            node.Attributes.Add(ParserConstants.SymbolTableEntry, entry);
                            node.GetNodeForKey(ParserConstants.Identifier).Attributes.Add(ParserConstants.SymbolTableEntry, entry);
                            node.GetNodeForKey(ParserConstants.Identifier).Attributes.Add(ParserConstants.SyntaxTreeNode, new IdentifierASTNode() { SymbolTableEntry = entry });
                        }),
                        new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.FunctionDeclarationArguments },
                        new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.Codeblock },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            FunctionASTNode syntaxTreeNode = new FunctionASTNode();
                            syntaxTreeNode.Arguments = node.GetAttributeForKey<List<SymbolTableEntry>>(ParserConstants.FunctionDeclarationArguments, Entries);
                            syntaxTreeNode.ReturnType = node.GetAttributeForKey<TypeASTNode>(ParserConstants.Type, ParserConstants.SyntaxTreeNode);
                            syntaxTreeNode.Body = node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.Codeblock, ParserConstants.SyntaxTreeNode);
                            syntaxTreeNode.FunctionName = node.GetAttributeForKey<SymbolTableEntry>(ParserConstants.Identifier, ParserConstants.SymbolTableEntry);

                            SymbolTableEntry entry = node.GetAttribute<SymbolTableEntry>(ParserConstants.SymbolTableEntry);
                            entry.Metadata.Add("FunctionASTNode", syntaxTreeNode);

                            node.Attributes[ParserConstants.SyntaxTreeNode] = syntaxTreeNode;
                        })
                    }
                )
            ));


            grammar.Add(new Production(ParserConstants.FunctionDeclarationArguments,
                new List<SubProduction>()
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.FunctionDeclarationArguments },
                            new TerminalExpressionDefinition { TokenType = TokenType.Comma },
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.FunctionDeclarationArgument },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                List<SymbolTableEntry> result = new List<SymbolTableEntry>();

                                List<SymbolTableEntry> entries = node.GetAttributeForKey<List<SymbolTableEntry>>(ParserConstants.FunctionDeclarationArguments, Entries);
                                SymbolTableEntry entry = node.GetAttributeForKey<SymbolTableEntry>(ParserConstants.FunctionDeclarationArgument, Entry);

                                result.AddRange(entries);
                                result.Add(entry);

                                node.Attributes.Add(Entries, result);
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.FunctionDeclarationArgument },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                List<SymbolTableEntry> entries = new List<SymbolTableEntry>();
                                SymbolTableEntry entry = node.GetAttributeForKey<SymbolTableEntry>(ParserConstants.FunctionDeclarationArgument, Entry);
                                entries.Add(entry);
                                node.Attributes.Add(Entries, entries);
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes.Add(Entries, new List<SymbolTableEntry>());
                            })
                        }
                    )
                }
            ));


            grammar.Add(new Production(ParserConstants.FunctionDeclarationArgument,
                new List<SubProduction>()
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.Type },
                            new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                SymbolTable symbolTable = node.CurrentSymbolTable;

                                TypeASTNode type = node.GetAttributeForKey<TypeASTNode>(ParserConstants.Type, ParserConstants.SyntaxTreeNode);
                                string key = node.GetAttributeForKey<WordToken>("Identifier", ParserConstants.Token).Lexeme;
                                SymbolTableEntry entry = null;

                                if (type is DynamicTypeASTNode dynASTNode)
                                {
                                    entry = symbolTable.Create(key, SymbolTableEntryType.Class);
                                    entry.SpecificType = dynASTNode.SymbolTableEntry;
                                }
                                else if (type is StaticTypeASTNode staticTypeASTNode)
                                {
                                    entry = symbolTable.Create(key, staticTypeASTNode.SymbolTableEntryType);
                                }

                                node.Attributes.Add(Entry, entry);
                            })
                        }
                    )
                }
            ));
        }
    }
}