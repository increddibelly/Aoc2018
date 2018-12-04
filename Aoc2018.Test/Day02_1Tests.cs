using NUnit.Framework;

namespace Aoc2018.Test
{
    internal class Day02_1Tests
    {
        private Day02 cut = new Day02();

        [TestCase("abcdef", 0, 0)]
        [TestCase("bababc", 1, 1)]
        [TestCase("abbcde", 1, 0)]
        [TestCase("abcccd", 0, 1)]
        [TestCase("aabcdd", 2, 0)]
        [TestCase("abcdee", 1, 0)]
        [TestCase("ababab", 0, 2)]
        public void Testcases(string input, int expected2s, int expected3s)
        {
            var result = cut.PuzzleCore(input);
            Assert.AreEqual(expected2s, result.Twos);
            Assert.AreEqual(expected3s, result.Threes);
        }

        [TestCase(new[] { "abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab" }, 12)]
        public void TestCaseChecksum(string[] input, int expected)
        {
            var result = cut.Checksum(input);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Puzzle()
        {
            var result = cut.Checksum(Source.Day2);
            Assert.AreEqual(7350, result);
        }

        [Test]
        public void TestDiff()
        {
            var src = new [] {"abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz"};
            var result = cut.FindClosestMatch(src);

            Assert.NotNull(result);
            Assert.That(result.A[result.Position] != result.B[result.Position]);
        }
    }
}
