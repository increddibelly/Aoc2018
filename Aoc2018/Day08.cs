using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aoc2018
{
    public class Day08
    {
        public int Puzzle(Queue<int> source)
        {
            var root = Node.Parse(source);
            return root.Sum();
        }

        public int Puzzle2(Queue<int> source)
        {
            var root = Node.Parse(source);
            return root.Value();
        }
    }

    public class Node
    {
        public int Id { get; protected set; }
        public int ChildCount { get; protected set; }
        public List<Node> Children { get; protected set; }
        public int MetaDataCount { get; protected set; }
        public int[] MetaData { get; protected set; }

        public int Sum()
        {
            return Children.Sum(x => x.Sum()) + MetaData.Sum();
        }

        private static int _nextId = 0;
        private static int IdGenerator()
        {
            _nextId++;
            return _nextId;
        }

        public Node Find(int id)
        {
            if (this.Id == id) return this ;

            return Children.Select(x => x.Find(id)).FirstOrDefault(x => x != null);
        }

        public int Value()
        {
            if (ChildCount == 0)
            {
                return MetaData.Sum();
            }

            var sum = 0;
            var dataSet = 
                MetaData.GroupBy(x => x)
                        .ToDictionary(x => x.Key, x => x.Count());

            foreach (var set in dataSet)
            {
                var index = set.Key - 1;

                if (ChildCount <= index || ChildCount < 1)
                    continue;

                // calculate once
                var childValue = Children[index].Value();
                sum += set.Value * childValue;
            }

            return sum;
        }

        public static Node Parse(Queue<int> input)
        {
            // 2 3  0 3  10 11 12  1 1  0 1 99  2  1 1 2
            // A---------------------------------------A
            //      B-----------B  C------------C
            //                          D---D

            var childCount = input.Dequeue(); // 2
            var metaDataCount = input.Dequeue(); // 3
            
            // store what we have.
            var node = new Node
            {
                Id = IdGenerator(),
                ChildCount = childCount,
                MetaDataCount = metaDataCount,
                Children = new List<Node>(childCount)
            };

            // parse children before parsing this.metadata
            for (var child = 0; child < childCount; child++)
            {
                var childNode = Parse(input);
                node.Children.Add(childNode);
            }

            var metaData = new List<int>(2*metaDataCount);
            for (var i=0; i<metaDataCount; i++)
            {
                metaData.Add(input.Dequeue());
            }

            node.MetaData = metaData.ToArray();

            return node;

        }

        public static void Reset()
        {
            _nextId = 0;
        }
    }

    public static class extensions
    {
        public static Queue<int> ToQueue(this string source)
        {
            return new Queue<int>(source.Split(" ").Select(int.Parse));
        }
    }
}
