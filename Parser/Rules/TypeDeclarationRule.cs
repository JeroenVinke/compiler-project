using Compiler.Common;
using Compiler.Parser;
using System.Collections.Generic;

namespace Compiler.Parser.Rules
{
    public class TypeDeclarationRule
    {
        //public static void Initialize(ref Grammar grammar)
        //{
        //    grammar.Add(new Production("TypeDeclaration",
        //        new SubProduction
        //        (
        //            new List<ExpressionDefinition>
        //            {
        //                new TerminalExpressionDefinition { TokenType = TokenType.TypeDeclaration  },
        //                new SemanticAction((ParsingNode node) =>
        //                {
        //                    node.Attributes.Add("type", node.GetAttributeForKey<WordToken>("TypeDeclaration", "token").Lexeme);
        //                    node.Attributes.Add("syntaxtreenode", new SyntaxTreeLeaf(node));
        //                })
        //            }
        //        )
        //    ));
        //}
    }
}