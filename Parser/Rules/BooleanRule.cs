using Compiler.Common;
using Compiler.Parser.SyntaxTreeNodes;
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
                    BooleanRules()
                }
            ));
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