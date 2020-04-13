using Compiler.Common;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class StatementRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            //grammar.Add(new Production("OpenStatement",
            //    new List<SubProduction>
            //    {
            //        new SubProduction
            //        (
            //            new List<ExpressionDefinition>
            //            {
            //                new NonTerminalExpressionDefinition { Identifier = "Statement``" },
            //                new SemanticAction((ParsingNode node) =>
            //                {
            //                    node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementASTNode>("Statement``", "syntaxtreenode");
            //                })
            //            }
            //        )
            //    }
            //));

            //grammar.Add(new Production("If",
            //    new List<SubProduction>
            //    {
            //        If()
            //    }
            //));

            //grammar.Add(new Production("If`",
            //    new List<SubProduction>
            //    {
            //        If(),
            //        EmptyIfRule()
            //    }
            //));

            grammar.Add(new Production("Statement",
                new List<SubProduction>
                {
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "OpenStatement" },
                            //new SemanticAction((ParsingNode node) =>
                            //{
                            //    node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementASTNode>("OpenStatement", "syntaxtreenode");
                            //})
                        }
                    ),
                    new SubProduction
                    (
                        new List<ExpressionDefinition>
                        {
                            new NonTerminalExpressionDefinition { Identifier = "ClosedStatement" },
                            //new SemanticAction((ParsingNode node) =>
                            //{
                            //    node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementASTNode>("ClosedStatement", "syntaxtreenode");
                            //})
                        }
                    )
                }
            ));

            grammar.Add(new Production("OpenStatement",
                new List<SubProduction>
                {
                    If(),
                    IfElseOpen()
                }
            ));

            grammar.Add(new Production("ClosedStatement",
                new List<SubProduction>
                {
                    Declaration(),
                    Codeblock(),
                    Assignment(),
                    //While(),
                    IfElseClosed(),
                    //NumericExpression()
                }
            ));

            //grammar.Add(new Production("Else",
            //    new List<SubProduction>
            //    {
            //        ElseIf(),
            //        EmptyElseRule()
            //    }
            //));
        }

        private static SubProduction While()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.While },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = "Codeblock" },
                    //new SemanticAction((ParsingNode node) =>
                    //{
                    //    WhileASTNode syntaxTreeNode = new WhileASTNode();
                    //    syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                    //    syntaxTreeNode.Body = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    //    node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    //})
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
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = "Statement" },
                    //new SemanticAction((ParsingNode node) =>
                    //{
                    //    IfASTNode syntaxTreeNode = new IfASTNode();
                    //    syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                    //    syntaxTreeNode.Body = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    //    node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    //})
                }
            );
        }

        private static SubProduction IfElseOpen()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.If },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisOpen },
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = "ClosedStatement" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Else },
                    new NonTerminalExpressionDefinition { Identifier = "OpenStatement" },
                    //new SemanticActionDefinition((ParsingNode node) =>
                    //{
                    //    IfASTNode syntaxTreeNode = new IfASTNode();
                    //    syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                    //    syntaxTreeNode.Body = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    //    node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    //})
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
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.ParenthesisClose },
                    new NonTerminalExpressionDefinition { Identifier = "ClosedStatement" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Else },
                    new NonTerminalExpressionDefinition { Identifier = "ClosedStatement" },
                    //new SemanticActionDefinition((ParsingNode node) =>
                    //{
                    //    IfASTNode syntaxTreeNode = new IfASTNode();
                    //    syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
                    //    syntaxTreeNode.Body = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    //    node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    //})
                }
            );
        }

        //private static SubProduction ElseIf()
        //{
        //    return new SubProduction
        //    (
        //        new List<ExpressionDefinition>
        //        {
        //            new TerminalExpressionDefinition { TokenType = TokenType.Else },
        //            new NonTerminalExpressionDefinition { Identifier = "If`" },
        //            new SemanticAction((ParsingNode node) =>
        //            {
        //                ;
        //                //IfASTNode syntaxTreeNode = new IfASTNode();
        //                //syntaxTreeNode.Condition = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression", "syntaxtreenode");
        //                //syntaxTreeNode.Body = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
        //                //node.Attributes["syntaxtreenode"] = syntaxTreeNode;
        //            })
        //        }
        //    );
        //}

        //private static SubProduction EmptyElseRule()
        //{
        //    return new SubProduction
        //    (
        //        new List<ExpressionDefinition>
        //        {
        //            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
        //            new SemanticAction((ParsingNode node) => {
        //                ;
        //                //node.Attributes.Add("syntaxtreenode", node.GetAttribute<NumericExpressionASTNode>("inh"));
        //                //node.Attributes.Add("syn", node.GetAttribute<NumericExpressionASTNode>("inh"));
        //            })
        //        }
        //    );
        //}

        //private static SubProduction EmptyIfRule()
        //{
        //    return new SubProduction
        //    (
        //        new List<ExpressionDefinition>
        //        {
        //            new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
        //            new SemanticAction((ParsingNode node) => {
        //                ;
        //                //node.Attributes.Add("syntaxtreenode", node.GetAttribute<NumericExpressionASTNode>("inh"));
        //                //node.Attributes.Add("syn", node.GetAttribute<NumericExpressionASTNode>("inh"));
        //            })
        //        }
        //    );
        //}

        private static SubProduction Assignment()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Identifier" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Assignment },
                    new NonTerminalExpressionDefinition { Identifier = "Factor" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
                    //new SemanticAction((ParsingNode node) =>
                    //{
                    //    AssignmentASTNode syntaxTreeNode = new AssignmentASTNode();
                    //    syntaxTreeNode.SymbolTableEntry = node.GetAttributeForKey<SymbolTableEntry>("Identifier", "symboltableentry");
                    //    syntaxTreeNode.Value = node.GetAttributeForKey<NumericExpressionASTNode>("NumericExpression", "syntaxtreenode");
                    //    node.Attributes["syntaxtreenode"] = syntaxTreeNode;
                    //})
                }
            );
        }

        private static SubProduction Codeblock()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Codeblock" },
                    //new SemanticAction((ParsingNode node) =>
                    //{
                    //    node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    //})
                }
            );
        }

        private static SubProduction NumericExpression()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "NumericExpression" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
                    //new SemanticAction((ParsingNode node) =>
                    //{
                    //    node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<StatementsASTNode>("Codeblock", "syntaxtreenode");
                    //})
                }
            );
        }

        private static SubProduction Declaration()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Declaration" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Semicolon }
                    //new SemanticAction((ParsingNode node) =>
                    //{
                    //    node.Attributes["syntaxtreenode"] = node.GetAttributeForKey<SyntaxTreeNode>("Declaration", "syntaxtreenode");
                    //}),
                }
            );
        }
    }
}
