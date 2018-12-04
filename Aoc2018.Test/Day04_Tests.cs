using System;
using System.Linq;
using NUnit.Framework;

namespace Aoc2018.Test
{
    [TestFixture]
    class Day04_Tests
    {
        private static string[] _source = new[]
        {
            "[1518-11-01 00:00] Guard #10 begins shift",
            "[1518-11-01 00:05] falls asleep",
            "[1518-11-01 00:25] wakes up",
            "[1518-11-01 00:30] falls asleep",
            "[1518-11-01 00:55] wakes up",
            "[1518-11-01 23:58] Guard #99 begins shift",
            "[1518-11-02 00:40] falls asleep",
            "[1518-11-02 00:50] wakes up",
            "[1518-11-03 00:05] Guard #10 begins shift",
            "[1518-11-03 00:24] falls asleep",
            "[1518-11-03 00:29] wakes up",
            "[1518-11-04 00:02] Guard #99 begins shift",
            "[1518-11-04 00:36] falls asleep",
            "[1518-11-04 00:46] wakes up",
            "[1518-11-05 00:03] Guard #99 begins shift",
            "[1518-11-05 00:45] falls asleep",
            "[1518-11-05 00:55] wakes up",
        };

        private Day04 _cut = new Day04();

        [Test]
        public void ShouldParse()
        {
            var result = _cut.Parse(_source);
            
            Assert.AreEqual(17, result.Length);
            foreach (var x in result)
            {
                Assert.That(x.Timestamp.Year == 2018);
                Assert.That(x.Timestamp.Month == 11);
            }

            Assert.IsTrue(result[1].FallAsleep);
            Assert.IsFalse(result[1].WakeUp);
            
            Assert.IsTrue(result[2].WakeUp);
            Assert.IsFalse(result[2].FallAsleep);

            Assert.That(result[5].Timestamp == new DateTime(2018, 11, 1, 23, 58, 0));

            Assert.That(result[0].Guard == 10);
            Assert.That(result[1].Guard == 10);
            Assert.That(result[2].Guard == 10);
            Assert.That(result[3].Guard == 10);
            Assert.That(result[4].Guard == 10);
            Assert.That(result[5].Guard == 99);
        }
    }
}
