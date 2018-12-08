using System.Linq;
using NUnit.Framework;

namespace Aoc2018.Test
{
    public class Day05_Tests
    {
        private readonly Day05 _cut = new Day05();

        [TestCase("aA", "")]
        [TestCase("abBA", "")]
        [TestCase("abAB", "abAB")]
        [TestCase("aabAAB", "aabAAB")]
        [TestCase("dabAcCaCBAcCcaDA", "dabCBAcaDA")]
        public void ShouldPuzzle(string input, string expected)
        {
            var result = _cut.Puzzle(input);

            Assert.AreEqual(expected, result);
        }

        [TestCase("aA", "")]
        [TestCase("abBA", "aA")]
        [TestCase("abAB", "abAB")]
        [TestCase("aabAAB", "aabAAB")]
        [TestCase("dabAcCaCBAcCcaDA", "dabAaCBAcCcaDA")]
        public void ShouldStep(string input, string expected)
        {
            var result = _cut.Step(input);

            Assert.AreEqual(expected, result);
        }

        [Test]
        [Ignore("Takes too damn long")]
        public void Puzzle()
        {
            var source = Source.Day5;

            var result = _cut.Puzzle(source);

            var answer = result.Length;
            Assert.AreEqual(9238, answer);
        }

        [Test]
        [Ignore("Takes too damn long")]
        public void Puzzle2()
        {
            var result = _cut.Puzzle2(Source.Day5);
            var max = result.OrderBy(x => x.Value.Length).First();
            Assert.AreEqual(max, 4052);
        }

        [TestCase("A", "dbCBcD")]
        [TestCase("B", "daCAcaDA")]
        [TestCase("C", "daDA")]
        [TestCase("D", "abCBAc")]
        public void ShouldStripAndRun(string toReplace, string expected)
        {
            var source = "dabAcCaCBAcCcaDA";

            var result = _cut.React(toReplace, source);

            Assert.AreEqual(expected, result);
        }

        [TestCase("A", "BCDbcd")]
        [TestCase("B", "ACDacd")]
        public void ShouldStrip(string input, string expected)
        {
            var result = _cut.Strip(input, "ABCDabcd");
            Assert.AreEqual(expected, result);
        }
    }
}
