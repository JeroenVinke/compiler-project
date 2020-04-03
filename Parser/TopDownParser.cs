//using Compiler.Common;
//using Compiler.LexicalAnalyer;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;

//namespace Compiler.Parser
//{
//    public class TopDownParser
//    {
//        public static List<ParsingTableEntry> ParsingTable { get; set; } = new List<ParsingTableEntry>();
        
//        public static List<Production> Grammar { get; set; }
//        public LexicalAnalyzer LexicalAnalyzer { get; set; }
//        public SyntaxTreeNode ParentSyntaxTreeNode { get; set; }
//        public ParsingNode ParentParsingTreeNode { get; set; } 
//        private Token Current { get; set; }
//        public SymbolTable RootSymbolTable { get; set; } = new SymbolTable();

//        public Parser(LexicalAnalyzer lexicalAnalyzer, List<Production> grammar)
//        {
//            LexicalAnalyzer = lexicalAnalyzer;
//            Grammar = grammar;
//        }

//        public void Parse()
//        {
//            ParsingTable = Compiler.Parser.ParsingTable.Create(Grammar);

//            Current = LexicalAnalyzer.GetNextToken();

//            Console.WriteLine("------ grammar ------");
//            foreach (Production production in Grammar)
//            {
//                Console.WriteLine(production);
//            }
//            Console.WriteLine("------ end of grammar ------");

//            ParsingStack Stack = new ParsingStack();
//            Stack.Add(new ParsingNode { Expression = new TerminalExpression { TokenType = TokenType.EndMarker }, Parser = this });

//            SubProduction p = GetAProduction();

//            ParsingNode parent = new ParsingNode
//            {
//                Expression = new NonTerminalExpression()
//                {
//                    Identifier = p.Production.Identifier
//                },
//                SubProduction = p,
//                Parser = this
//            };

//            Stack.InsertRange(0, p.Where(x => !(x is SemanticActionDefinition)).Select(x => new ParsingNode { Parent = parent, Expression = Expression.Create(x), SubProduction = p, Parser = this }));

//            Console.WriteLine("------ parsing ------");
//            while (Stack.First().Expression is NonTerminalExpression || (Stack.First().Expression is TerminalExpression te1 && te1.TokenType != TokenType.EndMarker))
//            {
//                if (Stack.First().Expression is TerminalExpression te && (te.TokenType == Current.Type || te.TokenType == TokenType.EmptyString))
//                {
//                    Stack.First().Attributes.Add("token", Current);

//                    Stack.RemoveAt(0);

//                    if (te.TokenType != TokenType.EmptyString)
//                    {
//                        Current = LexicalAnalyzer.GetNextToken();
//                    }
//                }
//                else if (Stack.First().Expression is TerminalExpression)
//                {
//                    throw new Exception();
//                }
//                else if (Stack.First().Expression is NonTerminalExpression nte)
//                {
//                    List<ParsingTableEntry> entries = ParsingTable.Where(x => x.Dimension1 == nte.Identifier && x.Dimension2.TokenType == Current.Type);

//                    SubProduction subProduction = entries.First().SubProduction;
//                    nte.Tree.SubProduction = subProduction;

//                    Stack.RemoveAt(0);

//                    Stack.InsertRange(0, subProduction.Where(x => !(x is SemanticActionDefinition)).Select(x => new ParsingNode { Parent = nte.Tree, Expression = Expression.Create(x), Parser = this }));
//                }
//            }
//            Console.WriteLine("------ end of parsing ------");

//            parent.EvaluateAttributes();

//            ParentParsingTreeNode = parent;

//            //Console.WriteLine("--------------------------------");
//            //Console.WriteLine(parent.GetAttribute<string>("code") + "\r\n" + end.ToString() + ":");
//            //Console.WriteLine("--------------------------------");

//            SyntaxTreeNode node = parent.GetAttribute<SyntaxTreeNode>("syntaxtreenode");
//            ParentSyntaxTreeNode = node;
//        }


//        public string Generate(string op, string value)
//        {
//            return $"{op} {value}";
//        }

//        public string Generate(string op, string addr1, string addr2)
//        {
//            return $"{addr1} {op} {addr2}";
//        }

//        private SubProduction GetAProduction()
//        {
//            foreach (Production p in Parser.Grammar)
//            {
//                foreach (SubProduction subProduction in p)
//                {
//                    if (subProduction.First(x => !(x is SemanticActionDefinition)).First().Any(x => x.TokenType == Current.Type))
//                    {
//                        return subProduction;
//                    }
//                }
//            }

//            return null;
//        }
//    }
//}