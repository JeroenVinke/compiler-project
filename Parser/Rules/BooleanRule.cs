using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
using System;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class BooleanRule
    {
        public static void Initialize(ref Grammar grammar)
        {
            grammar.Add(new Production(ParserConstants.Boolean,
                new List<SubProduction>
                {
                    BooleanRules(),
                    RelopRules()
                }
            ));
        }

        private static SubProduction RelopRules()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new NonTerminalExpressionDefinition { Identifier = ParserConstants.Factor },
                    new TerminalExpressionDefinition { TokenType = TokenType.RelOp },
                    new NonTerminalExpressionDefinition { Key = "Factor2", Identifier = ParserConstants.Factor },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        string relOpToken = node.GetAttributeForKey<WordToken>("RelOp", ParserConstants.Token).Lexeme;
                        RelOp relOp;

                        if (relOpToken == "==")
                        {
                            relOp = RelOp.Equals;
                        }
                        else if (relOpToken == "!=")
                        {
                            relOp = RelOp.NotEquals;
                        }
                        else if (relOpToken == "<")
                        {
                            relOp = RelOp.LessThan;
                        }
                        else if (relOpToken == "<=")
                        {
                            relOp = RelOp.LessOrEqualThan;
                        }
                        else if (relOpToken == ">")
                        {
                            relOp = RelOp.GreaterThan;
                        }
                        else if (relOpToken == ">=")
                        {
                            relOp = RelOp.GreaterOrEqualThan;
                        }
                        else
                        {
                            throw new Exception();
                        }

                        FactorASTNode left = node.GetAttributeForKey<FactorASTNode>(ParserConstants.Factor, ParserConstants.SyntaxTreeNode);
                        FactorASTNode right = node.GetAttributeForKey<FactorASTNode>("Factor2", ParserConstants.SyntaxTreeNode);
                        RelOpASTNode syntaxTreeNode = new RelOpASTNode(left, relOp, right);

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, syntaxTreeNode);
                    })
                }
            );
        }

        private static SubProduction BooleanRules()
        {
            return new SubProduction
            (
                new List<ExpressionDefinition>
                {
                    new TerminalExpressionDefinition { TokenType = TokenType.Boolean },
                    new SemanticActionDefinition((ParsingNode node) =>
                    {
                        bool value = node.GetAttributeForKey<WordToken>(ParserConstants.Boolean, ParserConstants.Token).Lexeme.ToLower() == "false" ? false : true;

                        node.Attributes.Add(ParserConstants.SyntaxTreeNode, new BooleanASTNode(value) { });
                    })
                }
            );
        }
    }
}