using Compiler.Common;
using Compiler.LexicalAnalyer;
using Compiler.Parser;
using Compiler.Parser.Common;
using Compiler.Parser.Rules;
using System;
using System.Collections.Generic;
using System.IO;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Advanced();

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }

        private static void Advanced()
        {
            Dictionary<string, Func<string, Token>> lexLanguage = new Dictionary<string, Func<string, Token>>();
            lexLanguage.Add(" #", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\r#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("\n#", (string value) => { return new WordToken { Type = TokenType.Nothing }; });
            lexLanguage.Add("public#", (string value) => { return new WordToken { Type = TokenType.Public }; });
            lexLanguage.Add("=#", (string value) => { return new WordToken { Type = TokenType.Assignment }; });
            lexLanguage.Add("(<|>|<=|>=|==|!=)#", (string value) => { return new WordToken { Type = TokenType.RelOp }; });
            lexLanguage.Add("\\|\\|#", (string value) => { return new WordToken { Type = TokenType.Or }; });
            lexLanguage.Add("&&#", (string value) => { return new WordToken { Type = TokenType.And }; });
            lexLanguage.Add("\\+#", (string value) => { return new WordToken { Type = TokenType.Plus }; });
            lexLanguage.Add("\\-#", (string value) => { return new WordToken { Type = TokenType.Minus }; });
            lexLanguage.Add("\\*#", (string value) => { return new WordToken { Type = TokenType.Multiplication }; });
            lexLanguage.Add("\\/#", (string value) => { return new WordToken { Type = TokenType.Division }; });
            lexLanguage.Add("while#", (string value) => { return new WordToken { Type = TokenType.While }; });
            lexLanguage.Add("(true|false)#", (string value) => { return new WordToken { Type = TokenType.Boolean }; });
            lexLanguage.Add("(string|int|void)#", (string value) => { return new WordToken { Type = TokenType.TypeDeclaration }; });
            lexLanguage.Add("{#", (string value) =>
            {
                return new WordToken { Type = TokenType.BracketOpen };
            });
            lexLanguage.Add("}#", (string value) =>
            {
                return new WordToken { Type = TokenType.BracketClose };
            });
            lexLanguage.Add("\\(#", (string value) => { return new WordToken { Type = TokenType.ParenthesisOpen }; });
            lexLanguage.Add("\\)#", (string value) => { return new WordToken { Type = TokenType.ParenthesisClose }; });
            lexLanguage.Add("([a-zA-Z])+([a-zA-Z0-9])*#", (string value) =>
            {

                return new WordToken { Type = TokenType.Identifier, Lexeme = value };
            });
            lexLanguage.Add("([0-9])*#", (string value) => { return new WordToken { Type = TokenType.Integer }; });
            lexLanguage.Add(";#", (string value) => { return new WordToken { Type = TokenType.Semicolon }; });

            string input = File.ReadAllText("./numexpr.txt");

            LexicalAnalyzer analyzer = new LexicalAnalyzer(lexLanguage, input);

            Grammar grammar = new Grammar();

            Parser.Parser parser = new Parser.Parser(analyzer, grammar);

            CodeblockRule.Initialize(ref grammar);
            StatementsRule.Initialize(ref grammar);
            StatementRule.Initialize(ref grammar);
            DeclarationRule.Initialize(ref grammar);
            //TypeDeclarationRule.Initialize(ref grammar);
            BooleanExpressionRule.Initialize(ref grammar);
            BooleanRule.Initialize(ref grammar);
            NumericExpressionRule.Initialize(ref grammar);
            FactorRule.Initialize(ref grammar);

            foreach (Production production in grammar)
            {
                Console.WriteLine(production.ToString());
            }

            parser.Parse();

            Parser.SyntaxTreeNode ast = parser.ParentParsingTreeNode.GetAttribute<Parser.SyntaxTreeNode>("syntaxtreenode");

            List<Instruction> instructions = new List<Instruction>();
            ast.GenerateCode(instructions);

            foreach(Instruction instruction in instructions)
            {
                Console.WriteLine(instruction.GenerateCodeString());
            }

            string result = "";
            result += "digraph A {\r\n";
            result += "subgraph cluster_2 {\r\n";
            result += "label=\"Parser tree\";\r\n";
            result += parser.ParentParsingTreeNode.ToDot();
            result += "}\r\n";
            result += "subgraph cluster_3 {\r\n";
            result += "label=\"Syntax tree\";\r\n";
            result += ast.ToDot();
            result += "}\r\n";
            result += "subgraph cluster_4 {\r\n";
            result += "label=\"Symbol table\";\r\n";
            result += parser.RootSymbolTable.ToDot();
            result += "}\r\n";
            result += "}\r\n";

            File.WriteAllText("output.txt", result);
        }
    }
}
