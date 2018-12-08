using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Aoc2018.Test
{
    [TestFixture]
    class Day06_Tests
    {
        private readonly Day06 _cut = new Day06();

        [Test]
        public void ShouldMatchGivenCases()
        {
            var input = new[]
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9"
            };
            var result = Map.Parse(input);

            Assert.AreEqual(8, result.Width);
            Assert.AreEqual(9, result.Height);
            Assert.AreEqual(6, result.Count);
        }

        [Test]
        public void TestAtEdge()
        {
            var map = Map.Parse(new[] {"1,1", "2,2", "3,3"});
            var edge1 = map.IsAtEdge(map.Markers[0]);
            var edge2 = map.IsAtEdge(map.Markers[1]);
            var edge3 = map.IsAtEdge(map.Markers[2]);

            Assert.IsTrue(edge1);
            Assert.IsFalse(edge2);
            Assert.IsTrue(edge3);
        }

        [TestCase(0, 0, 0)]
        [TestCase(5, 0, null)]
        public void ShouldBelongTo(int x, int y, int? expectedindex)
        {
            var map = Map.Parse(new[] {"1,1", "8,3", "1, 6", "3, 4", "5, 5", "8, 9"});

            var result = map.MappedArea[x, y];
            if (expectedindex.HasValue)
                Assert.AreEqual(result, map.Markers[expectedindex.Value]);
            else 
                Assert.IsNull(result);
        }

        [TestCase("1,1", 1, 1)]
        [TestCase("331,121", 331, 121)]
        public void ShouldParse(string input, int expectedX, int expectedY)
        {
            var result = Coordinate.Parse(input);

            Assert.AreEqual(expectedX, result.X);
            Assert.AreEqual(expectedY, result.Y);
        }

        [Test]
        public void ShouldDetect17()
        {
            var result = _cut.Puzzle(new[] {"1,1", "8,3", "1, 6", "3, 4", "5, 5", "8, 9"});
            Assert.AreEqual(17, result.Last().Value);
        }

        [Test]
        public void Puzzle()
        {
            var source = Source.Day06;

            var result = _cut.Puzzle(source);

            Assert.AreEqual(10097, result.Last().Value);
        }
    }
}
