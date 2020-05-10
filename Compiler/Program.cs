using Compiler.CodeGeneration;
using Compiler.LexicalAnalyer;
using Compiler.Parser;
using System;
using System.Diagnostics;
using System.IO;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText(args[0]);

            Stopwatch sw = Stopwatch.StartNew();
            LexicalAnalyzer analyzer = new LexicalAnalyzer(LexicalLanguage.GetLanguage(), input);
            BottomUpParser parser = new BottomUpParser(analyzer);

            parser.Parse();

            CodeGenerator.Generate(parser.GetIL());

            sw.Stop();

            Console.WriteLine($"Done (took {sw.ElapsedMilliseconds} milliseconds)");

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
