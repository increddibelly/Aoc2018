using System;
using NUnit.Framework;

namespace Aoc2018.Test
{
    internal class Day02_2Tests
    {
        private Day02 cut = new Day02();

        [Test]
        public void TestDiff()
        {
            var src = new [] {"abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz"};
            var result = cut.FindClosestMatch(src);

            Assert.NotNull(result);
            Assert.That(result.A[result.Position] != result.B[result.Position]);
            Assert.That(result.A == "fghij");
            Assert.That(result.B == "fguij");
            Assert.That(result.Result == "fgij");
        }

        [Test]
        public void Puzzle()
        {
            var result = cut.FindClosestMatch(Source.Day2);

            Assert.That(result.Result == "wmlnjevbfodamyiqpucrhsukg");
        }
    }
}
