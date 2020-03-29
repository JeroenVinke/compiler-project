using Compiler.RegularExpressionEngine;
using NUnit.Framework;

namespace RegularExpressionEngine.Tests
{
    public class RegularExpressionEngineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("=", SimulationState.OnTrack)]
        [TestCase("==", SimulationState.Accepting)]
        [TestCase("==", SimulationState.Accepting)]
        [TestCase(">=", SimulationState.Accepting)]
        [TestCase(">", SimulationState.Accepting)]
        [TestCase("<", SimulationState.Accepting)]
        [TestCase("!=", SimulationState.Accepting)]
        [TestCase("!=", SimulationState.Accepting)]
        [TestCase("$$", SimulationState.OffTrack)]
        public void RelOps(string input, SimulationState expectedResult)
        {
            SimulationState result = RegexEngine.Simulate("(<|>|<=|>=|==|!=)#", input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("f", SimulationState.OnTrack)]
        [TestCase("foo", SimulationState.Accepting)]
        [TestCase("bar", SimulationState.Accepting)]
        [TestCase("baz", SimulationState.OffTrack)]
        public void Words(string input, SimulationState expectedResult)
        {
            SimulationState result = RegexEngine.Simulate("(foo|bar)#", input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("a", "[a-z]#", SimulationState.Accepting)]
        [TestCase("a", "[b-z]#", SimulationState.OffTrack)]
        [TestCase("a", "[0-9a-z]#", SimulationState.Accepting)]
        [TestCase("a", "[0-9b-z]#", SimulationState.OffTrack)]
        public void Ranges(string input, string regex, SimulationState expectedResult)
        {
            SimulationState result = RegexEngine.Simulate(regex, input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase("||", "\\|\\|#", SimulationState.Accepting)]
        [TestCase("|", "\\|\\|#", SimulationState.OnTrack)]
        [TestCase("foo", "\\|\\|#", SimulationState.OffTrack)]
        public void Or(string input, string regex, SimulationState expectedResult)
        {
            SimulationState result = RegexEngine.Simulate(regex, input);

            Assert.AreEqual(expectedResult, result);
        }
    }
}