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
    }

    public class Instructions
    {
        public Dictionary<string, Instruction> instructionList { get; protected set; }

        public Instruction Next
        {
            get { return 
                instructionList
                    .OrderBy(x => x.Key)
                    .Where(x => x.Value.Status != Status.Completed)
                    .FirstOrDefault(x => x.Value.CanRun)
                    .Value;
            }
        }

        private Instructions()
        {

        }

        public static Instructions Parse(string[] input)
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
                    dependency = new Instruction(dependencyKey);
                    instructions.instructionList.Add(dependencyKey, dependency);
                }
                
                if (!instructions.instructionList.TryGetValue(instrKey, out var instruction))
                {
                    instruction = new Instruction(instrKey);
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
        public string Id { get; protected set; }
        public Status Status { get; set; } = Status.New;
        public List<Instruction> Dependencies { get; set; } = new List<Instruction>();
        public bool CanRun => 
            Status != Status.Blocked 
            && 
            Dependencies.All(x => x.Status == Status.Completed);

        public void Run(Reporter reporter)
        {
            if (!CanRun) return;
            reporter = reporter ?? new Reporter();

            var deps = Dependencies
                .Where(x => x.Status != Status.Completed)
                .OrderBy(x => x.Id);
            foreach (var dep in deps)
            {
                dep.Run(reporter);
            }

            reporter.Append(Id);
            Status = Status.Completed;
        }

        public Instruction(string key)
        {
            Id = key;
        }
    }

    public enum Status : byte
    {
        New = 0,
        Blocked=5,
        Completed = 10
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
