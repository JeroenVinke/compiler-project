using Compiler.Common;
using Compiler.LexicalAnalyer;
using System;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public class BottomUpParser
    {
        public static List<Production> Grammar { get; set; }
        public LexicalAnalyzer LexicalAnalyzer { get; set; }
        private Token Current { get; set; }
        public SymbolTable RootSymbolTable { get; set; } = new SymbolTable();

        public BottomUpParser(LexicalAnalyzer lexicalAnalyzer, List<Production> grammar)
        {
            LexicalAnalyzer = lexicalAnalyzer;
            Grammar = grammar;
        }

        public void Parse()
        {
            Current = LexicalAnalyzer.GetNextToken();

            Console.WriteLine("------ grammar ------");
            foreach (Production production in Grammar)
            {
                Console.WriteLine(production);
            }
            Console.WriteLine("------ end of grammar ------");
        }
    }
}