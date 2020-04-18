﻿using Compiler.Common;
using Compiler.LexicalAnalyer;
using Compiler.Parser;
using System;
using System.Collections.Generic;
using System.IO;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText(args[0]);

            LexicalAnalyzer analyzer = new LexicalAnalyzer(GetLexicalLanguage(), input);
            BottomUpParser parser = new BottomUpParser(analyzer);

            parser
                .Parse()
                .OutputDebugFiles()
                .OutputIL();

            Console.WriteLine("Done");

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }

        private static Dictionary<string, Func<string, Token>> GetLexicalLanguage()
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
            lexLanguage.Add("if#", (string value) => { return new WordToken { Type = TokenType.If }; });
            lexLanguage.Add("else#", (string value) => { return new WordToken { Type = TokenType.Else }; });
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
            return lexLanguage;
        }
    }
}
