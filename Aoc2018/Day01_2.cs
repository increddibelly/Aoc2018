using System.Collections.Generic;

namespace Aoc2018
{
    public class Day01_2
    {
        public int? Puzzle(int[] source)
        {
            var hit = new List<int> {0};
            var result = 0;

            do
                foreach (var item in source)
                {
                    result += item;
                    if (hit.Contains(result))
                        return result;
                    else
                        hit.Add(result);
                }
            while (true);
        }
    }
}
