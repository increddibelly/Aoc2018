using System.Collections.Generic;
using NUnit.Framework;

namespace Aoc2018.Test
{
    class Day08_Tests
    {
        Day08 _cut;
        private readonly Queue<int> _testCase = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2".ToQueue();

        [SetUp]
        public void Setup()
        {
            _cut = new Day08();
            Node.Reset();
        }

        [Test]
        public void ShouldParse()
        {
            var root = Node.Parse(_testCase);
            Assert.AreEqual(2, root.ChildCount);
            Assert.AreEqual(3, root.MetaDataCount);
            Assert.AreEqual(1, root.MetaData[0]);
            Assert.AreEqual(1, root.MetaData[1]);
            Assert.AreEqual(2, root.MetaData[2]);
            Assert.AreEqual(1, root.Id);
            
            Assert.AreEqual(0, root.Children[0].ChildCount);
            Assert.AreEqual(3, root.Children[0].MetaDataCount);
            Assert.AreEqual(10, root.Children[0].MetaData[0]);
            Assert.AreEqual(11, root.Children[0].MetaData[1]);
            Assert.AreEqual(12, root.Children[0].MetaData[2]);
            Assert.AreEqual(2, root.Children[0].Id);

            Assert.AreEqual(1, root.Children[1].ChildCount);
            Assert.AreEqual(1, root.Children[1].MetaDataCount);
            Assert.AreEqual(2, root.Children[1].MetaData[0]);
            Assert.AreEqual(3, root.Children[1].Id);

            Assert.AreEqual(0, root.Children[1].Children[0].ChildCount);
            Assert.AreEqual(1, root.Children[1].Children[0].MetaDataCount);
            Assert.AreEqual(99, root.Children[1].Children[0].MetaData[0]);
            Assert.AreEqual(4, root.Children[1].Children[0].Id);

            Assert.AreEqual(138, root.Sum());
        }

        [Test]
        public void ShouldSolveTestcase()
        {
            var root = _cut.Puzzle(_testCase);
            Assert.AreEqual(138, root);
        }

        [Test]
        public void ShouldFind()
        {
            var root = Node.Parse(_testCase);
            var result1 = root.Find(1);
            var result2 = root.Find(2);
            var result3 = root.Find(3);
            var result4 = root.Find(4);
            var result5 = root.Find(5);

            Assert.AreEqual(1, result1.Id);
            Assert.AreEqual(2, result2.Id);
            Assert.AreEqual(3, result3.Id);
            Assert.AreEqual(4, result4.Id);
            Assert.AreEqual(null, result5);
        }

        [Test]
        public void Puzzle()
        {
            var result = _cut.Puzzle(Source.Day8);
            Assert.AreEqual(40309, result);
        }

        [Test]
        public void ShouldGetValueTestcase()
        {
            var root = Node.Parse(_testCase);
            var value = root.Value();
            Assert.AreEqual(66, value);
        }

        [Test]
        public void Puzzle2()
        {
            var result = _cut.Puzzle2(Source.Day8);
            Assert.AreEqual(28779, result);
        }
    }
}
