using Compiler.Parser;
using NUnit.Framework;

namespace RegularExpressionEngine.Tests
{
    public class GrammarTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Follow()
        {
            NonTerminalExpressionDefinition nte = new NonTerminalExpressionDefinition() { Identifier = "Statements" };
            
            var result = nte.Follow();
        }
    }
}