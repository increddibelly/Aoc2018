using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018
{
    public class Day07
    {
        public string Puzzle(string[] input)
        {
            var rep = new Reporter();
            var program = Instructions.Parse(input);

            var next = program.Next;
            do
            {
                next?.Run(rep);
                next = program.Next;
            } while (next != null);

            return rep.ToString();
        }

        public string Puzzle2(string[] input, int defaultDuration = 60)
        {
            var rep = new Reporter();
            var program = Instructions.Parse(input, defaultDuration);
            var t = 0;
            var running = new List<Instruction>(5);
            
            FetchWork(running, program);

            do
            {
                foreach (var instr in running)
                {
                    instr?.Step(rep);
                }

                var done = running.Where(x => x.Status == Status.Completed).ToArray();
                foreach (var item in done)
                {
                    running.Remove(item);
                }
                t++;
                
                FetchWork(running, program);
            } while (running.Any());
            rep.Append($" in {t} steps");
            return rep.ToString();
        }

        private static void FetchWork(List<Instruction> running, Instructions program)
        {
            do
            {
                var next = program.Next;
                if (next == null) return;
                
                next.Status = Status.Selected;
                running.Add(next);
            }
            while (running.Count < 5);
        }
    }

    public class Instructions
    {
        public Dictionary<string, Instruction> instructionList { get; protected set; }

        public Instruction Next
        {
            get { return 
                instructionList
                    .OrderBy(x => x.Key)
                    .Where(x => x.Value.Status == Status.New)
                    .FirstOrDefault(x => x.Value.CanRun)
                    .Value;
            }
        }

        private Instructions()
        {

        }

        public static Instructions Parse(string[] input, int defaultDuration = 60)
        {
            var ordered = input.OrderBy(x => x).ToArray();

            var instructions = new Instructions();
            instructions.instructionList = new Dictionary<string, Instruction>();

            foreach (var line in ordered)
            {
                var dependencyKey = ParseDependencyKey(line);
                var instrKey = ParseInstructionKey(line);
                
                if (!instructions.instructionList.TryGetValue(dependencyKey, out var dependency))
                {
                    dependency = new Instruction(dependencyKey, defaultDuration);
                    instructions.instructionList.Add(dependencyKey, dependency);
                }
                
                if (!instructions.instructionList.TryGetValue(instrKey, out var instruction))
                {
                    instruction = new Instruction(instrKey, defaultDuration);
                    instructions.instructionList.Add(instrKey, instruction);
                }

                instruction.Dependencies.Add(dependency);
            }

            return instructions;
        }

        private static string ParseDependencyKey(string line)
        {
            // Step C must be finished before step A can begin.
            // should return C
            var items = line.Split(" ");
            return items[1];
        }
        
        private static string ParseInstructionKey(string line)
        {
            // Step C must be finished before step A can begin.
            // should return A
            var items = line.Split(" ");
            return items[7];
        }
    }

    public class Instruction
    {
        private int _baseDuration;
        public string Id { get; protected set; }
        public Status Status { get; set; } = Status.New;
        public List<Instruction> Dependencies { get; set; } = new List<Instruction>();
        public bool CanRun => 
            Status != Status.Running 
            && 
            Dependencies.All(x => x.Status == Status.Completed);

        public void Run(Reporter reporter)
        {
            if (!CanRun) return;

            reporter.Append(Id);
            Status = Status.Completed;
        }

        public void Step(Reporter reporter)
        {
            if (!CanRun && Status != Status.Running) return;

            if (Status == Status.Selected)
            {
                reporter.Append(Id); // log work start
                Status = Status.Running;
            }

            ToDo--;

            if (ToDo == 0)
            {
                Status = Status.Completed;
            }
        }

        public int Duration => _baseDuration + (byte) Id[0] - 64;
        public int ToDo { get; set; }

        public Instruction(string key, int defaultDuration = 60)
        {
            Id = key;
            _baseDuration = defaultDuration;
            ToDo = Duration;
        }

        public new string ToString()
        {

            return $"{Id} - {Status} - {Dependencies?.Count} - can {(CanRun ? "run" : "not run")}";
        }
    }

    public enum Status : byte
    {
        New = 0,
        Running=5,
        Completed = 10,
        Selected
    }

    public class Reporter
    {
        private readonly StringBuilder Report = new StringBuilder();

        public void Append(string x)
        {
            Report.Append(x);
        }

        public new string ToString()
        {
            return Report.ToString();
        }
    }
}
