using Compiler.CodeGeneration;
using Compiler.Common;
using Compiler.Common.Instructions;
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

            LexicalAnalyzer analyzer = new LexicalAnalyzer(LexicalLanguage.GetLanguage(), input);
            BottomUpParser parser = new BottomUpParser(analyzer);

            parser
                .OutputGrammar()
                .OutputAutomaton()
                .Parse()
                .OutputDebugFiles()
                .OutputIL();

            LiveVariableAnalysis.Analyse(parser.GetIL());

            Console.WriteLine("Done");

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
