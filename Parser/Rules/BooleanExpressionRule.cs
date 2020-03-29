using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class BooleanExpressionRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production("BooleanExpression", new SubProduction(
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = "Boolean" },
                    new SemanticAction((ParsingNode node) =>
                    {
                        node.GetNodeForKey("BooleanExpression`").Attributes.Add("inh", node.GetAttributeForKey<FactorASTNode>("Boolean", "syntaxtreenode"));
                    }),
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression`" },
                    new SemanticAction((ParsingNode node) =>
                    {
                        node.Attributes.Add("syntaxtreenode", node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression`", "syntaxtreenode"));
                    })
                })
            ));

            grammar.Add(new Production("BooleanExpression`",
                new List<SubProduction>
                {
                    OrRule(),
                    //AndRule(),
                    EmptyRule()
                }
            ));
        }

        //private static SubProduction AndRule()
        //{
        //    return new SubProduction
        //    (
        //        new List<ExpressionDefinition>
        //        {
        //            new TerminalExpressionDefinition { TokenType = TokenType.And },
        //            new NonTerminalExpressionDefinition { Identifier = "Boolean" },
        //            new NonTerminalExpressionDefinition { Identifier = "BooleanExpression`" },
        //            new SemanticAction((ParsingNode node) =>
        //            {
        //                SyntaxTreeNode syntaxTreeNode = new SyntaxTreeNode(SyntaxTreeNodeType.And, node);
        //                syntaxTreeNode.AddChild(node.GetAttributeForKey<SyntaxTreeNode>("Boolean", "syntaxtreenode"));
        //                syntaxTreeNode.AddChildren(node.GetAttributeForKey<List<SyntaxTreeNode>>("BooleanExpression`", "syntaxtreenode"));
        //                node.Attributes.Add("syntaxtreenode", syntaxTreeNode);

        //                node.Attributes.Add("code", $"{node.GetAttributeForKey<string>("Boolean", "code")}\r\n{node.GetAttributeForKey<Label>("Boolean", "false")}\r\n" +
        //                    $"{node.GetAttributeForKey<string>("BooleanExpression`", "code")}");
        //            })
        //        }
        //    );
        //}

        private static SubProduction EmptyRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.EmptyString },
                    new SemanticAction((ParsingNode node) => {
                        node.Attributes.Add("syntaxtreenode", node.GetAttribute<BooleanExpressionASTNode>("inh"));
                        node.Attributes.Add("syn", node.GetAttribute<BooleanExpressionASTNode>("inh"));
                    })
                }
            );
        }

        private static SubProduction OrRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Or },
                    new NonTerminalExpressionDefinition { Identifier = "Boolean" },
                    new SemanticAction((ParsingNode node) =>
                    {
                        node.GetNodeForKey("BooleanExpression`").Attributes.Add("inh", node.GetAttributeForKey<FactorASTNode>("Boolean", "syntaxtreenode"));
                    }),
                    new NonTerminalExpressionDefinition { Identifier = "BooleanExpression`" },
                    new SemanticAction((ParsingNode node) =>
                    {
                        BooleanExpressionASTNode left = node.GetAttribute<BooleanExpressionASTNode>("inh");
                        BooleanExpressionASTNode right = node.GetAttributeForKey<BooleanExpressionASTNode>("BooleanExpression`", "syntaxtreenode");

                        OrASTNode syntaxTreeNode = new OrASTNode(left, right);
                        node.Attributes.Add("syntaxtreenode", syntaxTreeNode);
                    })
                }
            );
        }    
    }
}