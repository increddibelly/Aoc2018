using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            var result = Map.Parse(TestCaseMap);

            Assert.AreEqual(8, result.Width);
            Assert.AreEqual(9, result.Height);
        }

        [Test]
        public void TestAtEdge()
        {
            var map = Map.Parse(TestCaseMap);
            var m = _cut.CreateMap(map);

            var markers = map.Markers.ToDictionary(x => x.Marker);
            Assert.IsFalse(map.IsAtEdge(markers['D']));
            Assert.IsFalse(map.IsAtEdge(markers['E']));

            Assert.IsTrue(map.IsAtEdge(markers['A']));
            Assert.IsTrue(map.IsAtEdge(markers['B']));
            Assert.IsTrue(map.IsAtEdge(markers['C']));
            Assert.IsTrue(map.IsAtEdge(markers['F']));
        }

        [TestCase(0, 0, 0)]
        [TestCase(5, 0, null)]
        public void ShouldBelongTo(int x, int y, int? expectedindex)
        {
            var map = Map.Parse(TestCaseMap);

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
        public void ShouldDetect9and17()
        {
            var source = TestCaseMap;
            var result = _cut.Puzzle(source);
            var view = _cut.CreateMap(Map.Parse(source));
            Assert.AreEqual(9, result.First().Value);
            Assert.AreEqual(17, result.Last().Value);
        }

        [Test]
        public void Puzzle()
        {
            var source = Source.Day6;

            var result = _cut.Puzzle(source);
            var map = _cut.CreateMap(Map.Parse(source));
            
            Assert.AreEqual(4342, result.Last().Value);

            var filename = @"C:\repos\aoc\Aoc2018\Day06.txt";
            File.WriteAllLines(filename, map);
        }

        [Test]
        public void Puzzle2GivenCase()
        {
            var map = Map.Parse(TestCaseMap);
            var test = new Coordinate(4, 3);
            var totalDistance34 = map.SumOfDistances(4, 3);
            
            var x = map.Markers.ToDictionary(m => m.Marker);
            Assert.AreEqual(30, totalDistance34);
            Assert.AreEqual( 5, x['A'].ManhattanDistance(test.X, test.Y));
            Assert.AreEqual( 6, x['B'].ManhattanDistance(test.X, test.Y));
            Assert.AreEqual( 4, x['C'].ManhattanDistance(test.X, test.Y));
            Assert.AreEqual( 2, x['D'].ManhattanDistance(test.X, test.Y));
            Assert.AreEqual( 3, x['E'].ManhattanDistance(test.X, test.Y));
            Assert.AreEqual(10, x['F'].ManhattanDistance(test.X, test.Y));
        }

        [Test]
        public void MarkerDistances()
        {
            var map = Map.Parse(TestCaseMap);
            var result = map.GetMarkerDistancesBelow(32);

            Assert.AreEqual(16, result);
        }

        [Test]
        public void Puzzle2()
        {
            var source = Source.Day6;
            var map = Map.Parse(source);

            var result = map.GetMarkerDistancesBelow(10000);
            Assert.AreEqual(42966, result);
        }

        private static readonly string[] TestCaseMap = new[] {"1,1", "1, 6", "8,3", "3, 4", "5, 5", "8, 9"};
    }
}
