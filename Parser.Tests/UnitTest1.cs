using Compiler.Common;
using Compiler.Parser;
using Compiler.Parser.Instances;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            Production production = new Production("Test", new List<SubProduction>()
            {
                new SubProduction(new List<ExpressionDefinition>()),
                new SubProduction(new List<ExpressionDefinition>())
            });

            Item item1 = new Item(
                production.First(),
                0,
                new HashSet<TerminalExpressionDefinition>(
                    new List<TerminalExpressionDefinition>
                    {
                        new TerminalExpressionDefinition() {
                            TokenType = TokenType.Class
                        }
                    }
                )
            );

            Item item2 = new Item(
                production.First(),
                0,
                new HashSet<TerminalExpressionDefinition>(
                    new List<TerminalExpressionDefinition>
                    {
                        new TerminalExpressionDefinition() {
                            TokenType = TokenType.Class
                        }
                    }
                )
            );

            Item item3 = new Item(
                production.First(),
                0,
                new HashSet<TerminalExpressionDefinition>(
                    new List<TerminalExpressionDefinition>
                    {
                        new TerminalExpressionDefinition() {
                            TokenType = TokenType.Comma
                        }
                    }
                )
            );

            Item item4 = new Item(
                production.First(),
                1,
                new HashSet<TerminalExpressionDefinition>(
                    new List<TerminalExpressionDefinition>
                    {
                        new TerminalExpressionDefinition() {
                            TokenType = TokenType.Class
                        }
                    }
                )
            );

            Item item5 = new Item(
                production.ElementAt(1),
                1,
                new HashSet<TerminalExpressionDefinition>(
                    new List<TerminalExpressionDefinition>
                    {
                        new TerminalExpressionDefinition() {
                            TokenType = TokenType.Class
                        }
                    }
                )
            );

            Assert.AreEqual(item1.GetHashCode(), item2.GetHashCode());
            Assert.AreNotEqual(item1.GetHashCode(), item3.GetHashCode());
            Assert.AreNotEqual(item1.GetHashCode(), item4.GetHashCode());
            Assert.AreNotEqual(item1.GetHashCode(), item5.GetHashCode());
        }
    }
}