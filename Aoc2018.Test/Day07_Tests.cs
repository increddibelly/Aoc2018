using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Aoc2018.Test
{
    class Day07_Tests
    {
        private Day07 _cut = new Day07();

        private readonly string[] _testcase = new[]
        {
            "Step C must be finished before step A can begin.",
            "Step C must be finished before step F can begin.",
            "Step A must be finished before step B can begin.",
            "Step A must be finished before step D can begin.",
            "Step B must be finished before step E can begin.",
            "Step D must be finished before step E can begin.",
            "Step F must be finished before step E can begin."
        };

        [Test]
        public void ShouldParseTestCase()
        {
            var result = Instructions.Parse(_testcase);
            var x = result.instructionList;

            Assert.IsFalse(x["C"].Dependencies.Any());
            Assert.IsTrue(x["A"].Dependencies.Any(d => d.Id == "C"));
            Assert.IsTrue(x["F"].Dependencies.Any(d => d.Id == "C"));
            Assert.IsTrue(x["B"].Dependencies.Any(d => d.Id == "A"));
            Assert.IsTrue(x["D"].Dependencies.Any(d => d.Id == "A"));
            Assert.IsTrue(x["E"].Dependencies.Any(d => d.Id == "B"));
            Assert.IsTrue(x["E"].Dependencies.Any(d => d.Id == "D"));
            Assert.IsTrue(x["E"].Dependencies.Any(d => d.Id == "F"));

            Assert.AreEqual(1, x["F"].Dependencies.Count);
            Assert.AreEqual(3, x["E"].Dependencies.Count);
            Assert.AreEqual(1, x["D"].Dependencies.Count);
            Assert.AreEqual(1, x["B"].Dependencies.Count);
            Assert.AreEqual(1, x["A"].Dependencies.Count);
            Assert.AreEqual(0, x["C"].Dependencies.Count);
        }

        
        [Test]
        public void ShouldRunTestCaseManualOrder()
        {
            var result = Instructions.Parse(_testcase);
            var x = result.instructionList;
            var reporter = new Reporter();
            var a = x["A"];
            var b = x["B"];
            var c = x["C"];
            var d = x["D"];
            var e = x["E"];
            var f = x["F"];

            Assert.IsTrue(c.CanRun);
            Assert.IsFalse(a.CanRun);
            Assert.IsFalse(b.CanRun);
            Assert.IsFalse(d.CanRun);
            Assert.IsFalse(e.CanRun);
            Assert.IsFalse(f.CanRun);
            // run step 1
            Assert.AreEqual(Status.New, c.Status);
            c.Run(reporter);
            Assert.AreEqual(Status.Completed, c.Status);
            // check end state step 1
            Assert.IsTrue(a.CanRun);
            Assert.IsFalse(b.CanRun);
            Assert.IsFalse(d.CanRun);
            Assert.IsFalse(e.CanRun);
            Assert.IsTrue(f.CanRun);
            
            // run step 2
            Assert.AreEqual(Status.New, a.Status);
            a.Run(reporter);
            Assert.AreEqual(Status.Completed, a.Status);
            // check end state step 2
            Assert.IsTrue(b.CanRun);
            Assert.IsTrue(d.CanRun);
            Assert.IsFalse(e.CanRun);
            Assert.IsTrue(f.CanRun);

            // run step 3
            Assert.AreEqual(Status.New, b.Status);
            b.Run(reporter);
            Assert.AreEqual(Status.Completed, b.Status);
            // check end state step 3
            Assert.IsTrue(d.CanRun);
            Assert.IsFalse(e.CanRun);
            Assert.IsTrue(f.CanRun);
            
            // run step 4
            Assert.AreEqual(Status.New, d.Status);
            d.Run(reporter);
            Assert.AreEqual(Status.Completed, d.Status);
            // check end state step 4
            Assert.IsFalse(e.CanRun);
            Assert.IsTrue(f.CanRun);

            // run step 5
            Assert.AreEqual(Status.New, f.Status);
            f.Run(reporter);
            Assert.AreEqual(Status.Completed, f.Status);
            // check end state step 5
            Assert.IsTrue(e.CanRun);

            // run step 6
            Assert.AreEqual(Status.New, e.Status);
            e.Run(reporter);
            Assert.AreEqual(Status.Completed, e.Status);

            Assert.IsTrue(x.All(instr => instr.Value.Status == Status.Completed));

            var output = reporter.ToString();
            Assert.AreEqual("CABDFE", output);
        }


        [Test]
        public void ShouldRunTestCase()
        {
            var result = _cut.Puzzle(_testcase);
            Assert.AreEqual("CABDFE", result);
        }

        [Test]
        public void CanRunTest()
        {
            var a = new Instruction("A");
            var b = new Instruction("B");
            var c = new Instruction("C");
            var rep = new Reporter();

            c.Dependencies.Add(b);
            b.Dependencies.Add(a);
            Assert.IsFalse(b.CanRun);
            Assert.IsFalse(c.CanRun);

            Assert.IsTrue(a.CanRun);
            a.Run(rep);
            Assert.IsTrue(b.CanRun);
            Assert.IsFalse(c.CanRun);
            b.Run(rep);
            Assert.IsTrue(c.CanRun);
            c.Run(rep);

            var result = rep.ToString();
            Assert.AreEqual("ABC", result);
        }
        
        [Test]
        public void CanNotRunTest()
        {
            var a = new Instruction("A");
            var b = new Instruction("B"){ Status = Status.Running};
            var c = new Instruction("C");
            var d = new Instruction("D");
            
            c.Dependencies.Add(b);
            b.Dependencies.Add(a);
            d.Dependencies.Add(b);
            d.Dependencies.Add(a);
            Assert.IsTrue(a.CanRun);
            Assert.IsFalse(b.CanRun);
            Assert.IsFalse(c.CanRun);
            Assert.IsFalse(d.CanRun);
        }

        [Test]
        public void RunPuzzle()
        {
            var result = _cut.Puzzle(Source.Day7);
            Assert.AreEqual("BITRAQVSGUWKXYHMZPOCDLJNFE", result);
        }


        [Test]
        public void RunPuzzle2()
        {
            var result = _cut.Puzzle2(Source.Day7);
            Assert.AreEqual("BTVYIWRSKMAQGZUXPHOCDLJNFE in 869 steps", result);
        }

        [Test]
        public void RunPuzzle2Example()
        {
            var result = _cut.Puzzle2(_testcase, 0);
            Assert.AreEqual("CAFBDE in 14 steps", result);
        }
        
        [TestCase("A", 0, 1)]
        [TestCase("Z", 0, 26)]
        [TestCase("A", 60, 61)]
        [TestCase("Z", 60, 86)]
        public void ShouldTakeNSeconds(string id, int baseDuration, int expectedDuration)
        {
            var instr = new Instruction(id, baseDuration);
            Assert.AreEqual(expectedDuration, instr.Duration);
        }
    }
}
