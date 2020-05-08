using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class StatementRule
    {
        public const string Factors = "Factors";
        public const string Factor = "Factor";

        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.Statement,
                new List<SubProduction>
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.OpenStatement },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes[ParserConstants.SyntaxTreeNode] = node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.OpenStatement, ParserConstants.SyntaxTreeNode);
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.ClosedStatement },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                node.Attributes[ParserConstants.SyntaxTreeNode] = node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.ClosedStatement, ParserConstants.SyntaxTreeNode);
                            })
                        }
                    )
                }
            ));

            grammar.Add(new Production(ParserConstants.OpenStatement,
                new List<SubProduction>
                {
                    If(),
                    IfElse()
                }
            ));

            grammar.Add(new Production(ParserConstants.ClosedStatement,
                new List<SubProduction>
                {
                    IfElseClosed(),
                    Declaration(),
                    Codeblock(),
                    Assignment(),
                    FunctionCall(),
                    Return(),
                    While()
                }
            ));

            grammar.Add(new Production(ParserConstants.Factors,
                new List<SubProduction>()
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factors },
                            new TerminalExpressionDefinition { TokenType = TokenType.Comma },
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factor },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                List<FactorASTNode> result = new List<FactorASTNode>();

                                List<FactorASTNode> factors = node.GetAttributeForKey<List<FactorASTNode>>(ParserConstants.Factors, Factors);
                                FactorASTNode factor = node.GetAttributeForKey<FactorASTNode>(ParserConstants.Factor, ParserConstants.SyntaxTreeNode);

                                result.AddRange(factors);
                                result.Add(factor);

                                node.Attributes.Add(Factors, result);
                            })
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factor },
                            new SemanticActionDefinition((ParsingNode node) =>
                            {
                                List<FactorASTNode> factors = new List<FactorASTNode>();
                                FactorASTNode factor = node.GetAttributeForKey<FactorASTNode>(ParserConstants.Factor, ParserConstants.SyntaxTreeNode);
                                factors.Add(factor);
                                node.Attributes.Add(Factors, factors);
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
                                node.Attributes.Add(Factors, new List<FactorASTNode>());
                            })
                        }
                    )
                }
            ));
        }

        private static SubProduction While()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.While },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Statement },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        WhileASTNode syntaxTreeNode = new WhileASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", ParserConstants.SyntaxTreeNode);
                        syntaxTreeNode.Body = node.GetAttributeForKey<SyntaxTreeNode>("Statement", ParserConstants.SyntaxTreeNode);
                        node.Attributes[ParserConstants.SyntaxTreeNode] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction If()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.If },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Statement },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        IfASTNode syntaxTreeNode = new IfASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", ParserConstants.SyntaxTreeNode);
                        syntaxTreeNode.Body = node.GetAttributeForKey<SyntaxTreeNode>("Statement", ParserConstants.SyntaxTreeNode);
                        node.Attributes[ParserConstants.SyntaxTreeNode] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction IfElse()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.If },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.ClosedStatement },
                    new TerminalExpressionDefinition { TokenType = TokenType.Else },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.OpenStatement },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        IfElseASTNode syntaxTreeNode = new IfElseASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>(ParserConstants.BooleanExpression, ParserConstants.SyntaxTreeNode);
                        syntaxTreeNode.IfBody = node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.ClosedStatement, ParserConstants.SyntaxTreeNode);
                        syntaxTreeNode.ElseBody = node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.OpenStatement, ParserConstants.SyntaxTreeNode);
                        node.Attributes[ParserConstants.SyntaxTreeNode] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction IfElseClosed()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.If },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.BooleanExpression },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Key = "ClosedStatement1", Identifier = ParserConstants.ClosedStatement },
                    new TerminalExpressionDefinition { TokenType = TokenType.Else },
                    new NonTerminalExpressionDefinition { Key = "ClosedStatement2", Identifier = ParserConstants.ClosedStatement },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        IfElseASTNode syntaxTreeNode = new IfElseASTNode();
                        syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>(ParserConstants.BooleanExpression, ParserConstants.SyntaxTreeNode);
                        syntaxTreeNode.IfBody = node.GetAttributeForKey<SyntaxTreeNode>("ClosedStatement1", ParserConstants.SyntaxTreeNode);
                        syntaxTreeNode.ElseBody = node.GetAttributeForKey<SyntaxTreeNode>("ClosedStatement2", ParserConstants.SyntaxTreeNode);
                        node.Attributes[ParserConstants.SyntaxTreeNode] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction FunctionCall()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factors },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
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

        private static SubProduction Assignment()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Identifier },
                    new TerminalExpressionDefinition { TokenType = TokenType.Assignment },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factor },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        AssignmentASTNode syntaxTreeNode = new AssignmentASTNode();
                        syntaxTreeNode.SymbolTableEntry = node.GetAttributeForKey<SymbolTableEntry>(ParserConstants.Identifier, ParserConstants.SymbolTableEntry);
                        syntaxTreeNode.Value = node.GetAttributeForKey<NumericExpressionASTNode>(ParserConstants.Factor, ParserConstants.SyntaxTreeNode);
                        node.Attributes[ParserConstants.SyntaxTreeNode] = syntaxTreeNode;
                    })
                }
            );
        }

        private static SubProduction Codeblock()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Codeblock },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes[ParserConstants.SyntaxTreeNode] = node.GetAttributeForKey<StatementsASTNode>(ParserConstants.Codeblock, ParserConstants.SyntaxTreeNode);
                    })
                }
            );
        }

        private static SubProduction Declaration()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Declaration },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        node.Attributes[ParserConstants.SyntaxTreeNode] = node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.Declaration, ParserConstants.SyntaxTreeNode);
                    }),
                }
            );
        }

        private static SubProduction Return()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Return },
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factor },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        ReturnASTNode astNode = new ReturnASTNode();
                        astNode.ReturnValue = node.GetAttributeForKey<SyntaxTreeNode>(ParserConstants.Factor, ParserConstants.SyntaxTreeNode);
                        node.Attributes[ParserConstants.SyntaxTreeNode] = astNode;
                    }),
                }
            );
        }
    }
}
