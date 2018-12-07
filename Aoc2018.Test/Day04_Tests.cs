using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace Aoc2018.Test
{
    [TestFixture]
    class Day04_Tests
    {
        private static readonly string[] _source = new[]
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
            var shifts = result.Split();
            
            Assert.AreEqual(17, result.Count);
            foreach (var x in result)
            {
                Assert.That(x.Timestamp.Year == 2018);
                Assert.That(x.Timestamp.Month == 11);
            }

            Assert.IsTrue(result[1].Type == Event.EventType.FallAsleep);
            
            Assert.IsTrue(result[2].Type == Event.EventType.WakeUp);

            Assert.That(result[5].Timestamp == new DateTime(2018, 11, 1, 23, 58, 0));

            Assert.That(result[0].Guard == 10);
            Assert.That(result[0].Type == Event.EventType.SetGuard);
            Assert.That(result[1].Guard == 10);
            Assert.That(result[2].Guard == 10);
            Assert.That(result[3].Guard == 10);
            Assert.That(result[4].Guard == 10);
            Assert.That(result[5].Guard == 99);

            Assert.That(shifts.Count == 4);
            Assert.That(shifts[0].SleepDuration == 45);
        }

        [Test]
        public void Puzzle()
        {
            var shifts = _cut.ToShifts(_cut.Parse(Source.Day4));
            var sleepsumPerGuard = shifts.ToDictionary(x => x.Key, x => x.Value.Sum(y => y.SleepDuration));
            var sleepiest = sleepsumPerGuard.OrderByDescending(x => x.Value).First();

            Assert.AreEqual(2663, sleepiest.Key);
            Assert.AreEqual(522, sleepsumPerGuard[sleepiest.Key]);

            var shiftsForSleepiestGuard = shifts[2663];

            var output = new Dictionary<int, int>();
            for (var i = 0; i <= 59; i++) output.Add(i, 0); // set all minutes to 0

            foreach (var shift in shiftsForSleepiestGuard)
            {
                for (var minute = 0; minute <= 59; minute++)
                {
                    output[minute] = output[minute] + shift.CountSleepsAt(minute);
                }
            }

            var max = output.OrderByDescending(x => x.Value).First();
            Assert.AreEqual(45, max.Key);
            Assert.AreEqual(15, max.Value);

            Assert.AreEqual(119835, sleepiest.Key * max.Key);
        }

        [Test]
        public void Puzzle2()
        {
            var shifts = _cut.ToShifts(_cut.Parse(Source.Day4));

            var sleepsPerMinutePerGuard = new Dictionary<int, Dictionary<int, int>>();
            foreach (var shiftsForGuard in shifts)
            {
                var guard = shiftsForGuard.Key;
                sleepsPerMinutePerGuard.Add(guard, new Dictionary<int, int>());

                foreach (var shift in shifts[guard])
                {
                    for (var minute = 0; minute <= 59; minute++)
                    {
                        if (!sleepsPerMinutePerGuard[guard].ContainsKey(minute))
                        {
                            sleepsPerMinutePerGuard[guard].Add(minute, 0);
                        }
                        sleepsPerMinutePerGuard[guard][minute] += shift.SleepsAt(minute) ? 1 : 0;
                    }
                }
            }
            
            var sleepiestMinutePerGuard = new Dictionary<int, int>();
            foreach (var item in sleepsPerMinutePerGuard)
            {
                var guard = item.Key;
                var sleepsPerMinute = item.Value;
                var maxSleepingAtMinute = sleepsPerMinute.OrderByDescending(x => x.Value).First();
                sleepiestMinutePerGuard.Add(guard, maxSleepingAtMinute.Key);
            }

            var agg = sleepiestMinutePerGuard.OrderByDescending(x => x.Value).ToArray();
            var output = agg.First();
            Assert.AreEqual(433, output.Key);
            Assert.AreEqual(49, output.Value);
        }

        [Test]
        public void Puzzle2_2()
        {
            var shiftsPerGuard = _cut.ToShifts(_cut.Parse(Source.Day4));

            var theMinute = -1;
            var theGuard = 0;
            var theCount = 0;

            foreach (var shift in shiftsPerGuard)
            {
                var guard = shift.Key;
                var record = _cut.TotalSleepsPerMinute(shift.Value);

                foreach (var minute in record)
                {
                    if (minute.Value > theCount)
                    {
                        theMinute = minute.Key;
                        theCount = minute.Value;
                        theGuard = guard;
                    }
                }
            }

            Assert.AreEqual(20, theCount);
            Assert.AreEqual(509, theGuard);
            Assert.AreEqual(25, theMinute);
            Assert.AreEqual(12725, theMinute * theGuard);
        }
    }
}
