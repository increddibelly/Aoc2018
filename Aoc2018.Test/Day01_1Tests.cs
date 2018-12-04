using NUnit.Framework;

namespace Aoc2018.Test
{
    internal class Day01_1Tests
    {
        private Day01_1 cut = new Day01_1();

        [TestCase(new[] { 1, 1, 1 }, 3)]
        [TestCase(new[] { 1, 1, -2 }, 0)]
        [TestCase(new[] { -1, -2, -3 }, -6)]
        public void Testcases(int[] source, int expected)
        {
            var result = cut.Puzzle(source);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RunPuzzle()
        {
            var result = cut.Puzzle(Source.Day1);
            Assert.AreEqual(522, result);
        }
    }
}