using NUnit.Framework;

namespace Aoc2018.Test
{
    internal class Day01_2Tests
    {
        private Day01_2 cut = new Day01_2();

        [TestCase(new[] {1, -1}, 0)]
        [TestCase(new[] { +3, +3, +4, -2, -4 }, 10)]
        [TestCase(new[] { -6, +3, +8, +5, -6 }, 5)]
        [TestCase(new[] { +7, +7, -2, -7, -4 }, 14)]
        public void Testcases(int[] source, int expected)
        {
            var result = cut.Puzzle(source);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RunPuzzle()
        {
            var result = cut.Puzzle(Source.Day1);
            Assert.AreEqual(73364, result);
        }
    }
}