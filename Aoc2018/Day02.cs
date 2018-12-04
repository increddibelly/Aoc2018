using System.Linq;

namespace Aoc2018
{
    public class Day02
    {
        public Day2CoreResult PuzzleCore(string input)
        {
            var detection = 
                input.ToCharArray()
                     .GroupBy(x => x)
                     .ToDictionary(x => x, x => x.Count());

            return new Day2CoreResult
            {
                Twos = detection.Count(x => x.Value == 2),
                Threes = detection.Count(x => x.Value == 3),
            };
        }

        public int Checksum(string[] input)
        {
            var maths =
                input.Select(PuzzleCore)
                     .ToList();

            var result = 
                maths.Count(x => x.Twos > 0)
                * 
                maths.Count(x => x.Threes > 0);

            return result;
        }

        private CompareResult Compare(string a, string b)
        {
            CompareResult result = null;
            for (var index = 0; index < a.Length; index++)
            {
                if (a[index] == b[index]) continue;
                if (result == null)
                {
                    result = new CompareResult
                    {
                        Position = index,
                        A = a,
                        B = b
                    };
                }
                else return null;

            }
            // 0 or 1 hits
            return result;
        }

        public CompareResult FindClosestMatch(string[] input)
        {
            var srcA = input.OrderBy(x => x).ToArray();
            var srcB = input.OrderBy(x => x).ToArray();

            foreach(var A in srcA)
            {
                foreach (var B in srcB)
                {
                    var result = Compare(A, B);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }

    public class CompareResult
    {
        public int Position { get; set; }
        public string A { get; set; }
        public string B { get; set; }

        public string Result => A?.Substring(0, Position) + B?.Substring(Position + 1);
    }

    public class Day2CoreResult
    {
        public int Twos { get; set; }
        public int Threes { get; set; }
    }
}
