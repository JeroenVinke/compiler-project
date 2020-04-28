using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class ClassRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.Classes,
                new List<SubProduction>
                {
                    new SubProduction(new List<ExpressionDefinition>()
                    {
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.Class },
                        new NonTerminalExpressionDefinition { Identifier = ParserConstants.Classes },
                        new SemanticActionDefinition((ParsingNode node) =>
                        {
                            ClassesASTNode astNode = new ClassesASTNode();
                            astNode.Classes.Add(node.GetAttributeForKey<ClassASTNode>(ParserConstants.Class, ParserConstants.SyntaxTreeNode));
                            astNode.Classes.AddRange(node.GetAttributeForKey<ClassesASTNode>(ParserConstants.Classes, ParserConstants.SyntaxTreeNode).Classes);
                            node.Attributes.Add(ParserConstants.SyntaxTreeNode, astNode);
                        })
                    }),
                    //new SubProduction(new List<ExpressionDefinition>()
                    //{
                    //    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Class },
                    //    new SemanticActionDefinition((ParsingNode node) =>
                    //    {
                    //        ClassesASTNode astNode = new ClassesASTNode();
                    //        astNode.Classes.Add(node.GetAttributeForKey<ClassASTNode>(ParserConstants.Class, ParserConstants.SyntaxTreeNode));
                    //        node.Attributes.Add(ParserConstants.SyntaxTreeNode, astNode);
                    //    })
                    //}),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes.Add(ParserConstants.SyntaxTreeNode, new ClassesASTNode());
                            })
                        }
                    )
                }
            ));

            grammar.Add(new Production("Class",
                new SubProduction(new List<ExpressionDefinition>()
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Class },
                    new TerminalExpressionDefinition { TokenType = TokenType.Identifier },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes[ParserConstants.SymTable] = node.ChildSymbolTable;

                        string key = node.GetAttributeForKey<WordToken>(ParserConstants.Identifier, ParserConstants.Token).Lexeme;
                        SymbolTableEntry entry = node.ChildSymbolTable.Create(key, SymbolTableEntryType.Class);

                        node.GetNodeForKey("Identifier").Attributes.Add(ParserConstants.SymbolTableEntry, entry);
                        node.GetNodeForKey("Identifier").Attributes.Add(ParserConstants.SyntaxTreeNode, new IdentifierASTNode() { SymbolTableEntry = entry });
                    }),
                    new TerminalExpressionDefinition { TokenType = TokenType.BracketOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.ClassMembers },
                    new TerminalExpressionDefinition { TokenType = TokenType.BracketClose },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        ClassASTNode classASTNode = new ClassASTNode();
                        classASTNode.ClassName = node.GetAttributeForKey<SymbolTableEntry>(ParserConstants.Identifier, ParserConstants.SymbolTableEntry);
                        classASTNode.Children = node.GetAttributeForKey<List<SyntaxTreeNode>>(ParserConstants.ClassMembers, ParserConstants.SyntaxTreeNodes);
                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, classASTNode);
                    })
                })
            ));
        }
    }
}