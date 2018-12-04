using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Aoc2018.Test
{
    class Day03_1Tests
    {
        private Day03 cut = new Day03();

        private string[] _source = new[]
        {
            "#1 @ 1,3: 4x4",
            "#2 @ 3,1: 4x4",
            "#3 @ 5,5: 2x2"
        };

        [TestCase("#1 @ 1,3: 4x4", 1, 1, 3, 4, 4)]
        [TestCase("#2 @ 3,1: 4x4", 2, 3, 1, 4, 4)]
        [TestCase("#3 @ 5,5: 2x2", 3, 5, 5, 2, 2)]
        public void TestParse(string src, int id, int x, int y, int width, int height)
        {
            var result = Claim.Parse(src);
            Assert.AreEqual(result.Id, id);
            Assert.AreEqual(result.Area.Left, x);
            Assert.AreEqual(result.Area.Top, y);
            Assert.AreEqual(result.Area.Right, x+width);
            Assert.AreEqual(result.Area.Bottom, y+height);
        }

        [TestCase(1,1, true)]
        [TestCase(1,2, true)]
        [TestCase(2,1, true)]
        [TestCase(1,3, false)]
        [TestCase(2,3, false)]
        [TestCase(3,1, false)]
        [TestCase(3,2, false)]
        [TestCase(3,3, true)]
        public void ShouldOverlap(int id1, int id2, bool expected)
        {
            var claims = CreateClaims().ToDictionary(x => x.Id);

            var actual = claims[id1].Covers(claims[id2]);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Puzzle()
        {
            var result = cut.Puzzle(Source.Day3);
            Assert.AreEqual(120408, result);
        }

        [Test]
        public void Puzzle2()
        {
            var result = cut.Puzzle2(Source.Day3);
            Assert.AreEqual(1276, result);
        }

        [Test]
        public void Puzzle2TestCases()
        {
            var result = cut.Puzzle2(_source);
            Assert.AreEqual(3, result);
        }

        private Claim[] CreateClaims()
        {
            return _source.Select(Claim.Parse).ToArray();
        }
    }
}
