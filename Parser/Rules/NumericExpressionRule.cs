﻿using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class NumericExpressionRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            //grammar.Add(new Production("NumericExpression",
            //    new SubProduction
            //    (
            //        new List<ExpressionDefinition>
            //        {
            //            new NonTerminalExpressionDefinition { Identifier = "Factor" },
            //            //new SemanticAction((ParsingNode node) =>
            //            //{
            //            //    node.GetNodeForKey("NumericExpression`").Attributes.Add("inh", node.GetAttributeForKey<FactorASTNode>("Factor", "syntaxtreenode"));
            //            //}),
            //            new NonTerminalExpressionDefinition { Identifier = "NumericExpression`" },
            //            //new SemanticAction((ParsingNode node) =>
            //            //{
            //            //    node.Attributes.Add("syntaxtreenode", node.GetAttributeForKey<NumericExpressionASTNode>("NumericExpression`", "syntaxtreenode"));
            //            //})
            //        }
            //    )
            //));

            grammar.Add(new Production("NumericExpression", new List<SubProduction> {
                PlusRule()
            }));
        }


        private static SubProduction PlusRule()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition> {
                    new NonTerminalExpressionDefinition { Identifier = "Factor" },
                    new TerminalExpressionDefinition { TokenType = TokenType.Plus },
                    new NonTerminalExpressionDefinition { Key="Factor2", Identifier = "Factor" },
                    //new SemanticAction((ParsingNode node) => {
                    //        node.GetNodeForKey("NumericExpression`").Attributes.Add("inh", node.GetAttributeForKey<SyntaxTreeNode>("Factor", "syntaxtreenode"));
                    //}),
                    //new NonTerminalExpressionDefinition { Identifier = "NumericExpression`" },
                    //new SemanticAction((ParsingNode node) => {
                    //    FactorASTNode left = node.GetAttribute<FactorASTNode>("inh");
                    //    FactorASTNode right = node.GetAttributeForKey<FactorASTNode>("NumericExpression`", "syntaxtreenode");
                    //    AdditionASTNode syntaxTreeNode = new AdditionASTNode(left, right);
                    //    node.Attributes.Add("syntaxtreenode", syntaxTreeNode);
                    //})
                }
            );
        }
    }
}