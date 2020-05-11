using Compiler.CodeGeneration;
using Compiler.CodeGeneration.Operations;
using Compiler.Common;
using Compiler.Common.Instructions;
using Compiler.LexicalAnalyer;
using Compiler.Parser;
using Compiler.RegularExpressionEngine;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JeroenCompilerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompilerController : ControllerBase
    {
        [HttpPost]
        public CompilationResult Compile([FromBody]CompilationRequest request)
        {
            CompilationResult result = new CompilationResult();
            LexicalAnalyzer analyzer = new LexicalAnalyzer(LexicalLanguage.GetLanguage(), request.Input);
            BottomUpParser parser = new BottomUpParser(analyzer);

            parser.Parse();

            result.IL = parser.GetILAsString();
            result.Assembly = new CodeGenerator().GenerateAsString(parser.GetIL());

            return result;
        }

        [HttpPost]
        [Route("ast")]
        public ASTResult AST([FromBody]CompilationRequest request)
        {
            ASTResult result = new ASTResult();

            LexicalAnalyzer analyzer = new LexicalAnalyzer(LexicalLanguage.GetLanguage(), request.Input);
            BottomUpParser parser = new BottomUpParser(analyzer);

            parser.Parse();

            result.AST += "digraph C {\r\n";
            result.AST += parser.TopLevelAST.ToDot();
            result.AST += "}";

            return result;
        }

        [HttpGet]
        [Route("automaton")]
        public AutomatonResult Automaton()
        {
            AutomatonResult result = new AutomatonResult();
            BottomUpParser parser = new BottomUpParser();

            result.Automaton += "digraph C {\r\n";
            result.Automaton += parser.GetAutomaton();
            result.Automaton += "}";

            return result;
        }

        [HttpGet]
        [Route("grammar")]
        public GrammarResult Grammar()
        {
            GrammarResult result = new GrammarResult();

            foreach (Production production in Compiler.Parser.Grammar.Instance)
            {
                ProductionModel productionModel = new ProductionModel { Head = production.Identifier };

                production.ForEach(x =>
                {
                    productionModel.SubProductions.Add(new SubProductionModel
                    {
                        Expressions = x.Select(y => new ExpressionModel
                        {
                            Name = y.ToString(),
                            IsNonTerminalExpression = y is NonTerminalExpressionDefinition
                        }).ToList()
                    });
                });

                result.Productions.Add(productionModel);
            }

            return result;
        }

        [HttpPost]
        [Route("regex")]
        public RegexResult Regex([FromBody]RegexRequest request)
        {
            RegexResult result = new RegexResult();

            Compiler.RegularExpressionEngine.SyntaxTreeNode syntaxTree = RegexToSyntaxTreeConverter.Convert(request.Regex);
            result.SyntaxTree += "digraph C {\r\n";
            result.SyntaxTree += syntaxTree.TopParent().ToString();
            result.SyntaxTree += "}\r\n";

            Node dfa = RegexToDFAConverter.Convert(request.Regex);
            result.DFA += "digraph D {\r\n";
            result.DFA += dfa.ToDot();
            result.DFA += "}\r\n";

            result.Matches = RegexEngine.Simulate(request.Regex, request.Input) == SimulationState.Accepting;

            return result;
        }

        [HttpPost]
        [Route("tokenize")]
        public LexicalAnalyzerResult Tokenize([FromBody]LexicalAnalyzerRequest request)
        {
            LexicalAnalyzerResult result = new LexicalAnalyzerResult();

            LexicalAnalyzer analyzer = new LexicalAnalyzer(LexicalLanguage.GetLanguage(), request.Input);

            Token current = analyzer.GetNextToken();
            result.Tokens.Add(TokenToModel(current));

            while(!(current is EndOfFileToken))
            {
                current = analyzer.GetNextToken();
                result.Tokens.Add(TokenToModel(current));
            }

            return result;
        }

        private TokenModel TokenToModel(Token token)
        {
            return new TokenModel
            {
                Type = Enum.GetName(typeof(TokenType), token.Type),
                Value = token is WordToken wordToken ? wordToken.Lexeme : "" + token.Character
            };
        }
    }

    public class GrammarResult
    {
        public List<ProductionModel> Productions { get; set; } = new List<ProductionModel>();
    }

    public class ProductionModel
    {
        public string Head { get; set; }

        public List<SubProductionModel> SubProductions = new List<SubProductionModel>();
    }

    public class SubProductionModel
    {
        public List<ExpressionModel> Expressions { get; set; }
    }

    public class ExpressionModel
    {
        public string Name { get; set; }
        public bool IsNonTerminalExpression { get; set; }
    }

    public class CompilationRequest
    {
        public string Input { get; set; }
    }

    public class CompilationResult
    {
        public string IL { get; set; }
        public string Assembly { get; set; }
    }

    public class LexicalAnalyzerRequest
    {
        public string Input { get; set; }
    }

    public class LexicalAnalyzerResult
    {
        public List<TokenModel> Tokens { get; set; } = new List<TokenModel>();
    }

    public class TokenModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class RegexRequest
    {
        public string Regex { get; set; }
        public string Input { get; set; }
    }

    public class RegexResult
    {
        public string SyntaxTree { get; set; }
        public string DFA { get; set; }
        public bool Matches { get; set; }
    }

    public class ASTResult
    {
        public string AST { get; set; }
    }

    public class AutomatonResult
    {
        public string Automaton { get; set; }
    }
}
