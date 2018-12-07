using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Aoc2018
{
    public class Day05
    {
        public string Puzzle(string input)
        {
            var current = input;
            string previous;

            do
            {
                previous = current;
                current = Step(current);
            } while (current != previous);
            
            return current;
        }

        public string Step(string input)
        {
            for(var L = 0; L <= input.Length - 2; L++)
            {
                var R = L + 1;
                if (ReactsWith(input[L], input[R]))
                {
                    return input.Remove(L, 2);
                }
            }

            return input;
        }

        public Dictionary<char, string> Puzzle2(string input)
        {
            var output = new Dictionary<char, string>();
            for (var x = 'Z'; x >= 'A'; x--)
            {
                var result = React(x.ToString(), input);
                output.Add(x, result);
                Debug.WriteLine($"{x} => {result.Length}");
            }
            return output;
        }

        public string React(string replace, string input)
        {
            var strippedInput = Strip(replace, input);
            var result = Puzzle(strippedInput);

            return result;
        }

        public string Strip(string target, string input)
        {
            return input.Replace(target, "", StringComparison.CurrentCultureIgnoreCase);
        }

        private bool ReactsWith(char left, char right)
        {
            // must be same (ignoring case) but different (= different case)
            return string.Equals(left.ToString(), right.ToString(), StringComparison.InvariantCultureIgnoreCase) 
                   && left != right; 
        }
    }
}
