using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Aoc2018
{
    public class Day03
    {
        private Dictionary<int, Claim> Parse(string[] source)
        {
            return source.Select(Claim.Parse).ToDictionary(x => x.Id);
        }

        public int Puzzle(string[] source)
        {
            var claimsById = Parse(source);
            var claims = claimsById.Values.ToArray();

            var doubleCovered = 0;

            var texture = new Rectangle(1, 1, claims.Max(x => x.Area.Right), claims.Max(x => x.Area.Bottom));
            var covercount = new int[texture.Width,texture.Height];

            foreach (var claim in claims)
            {
                for (var x = claim.Area.Left; x < claim.Area.Right; x++)
                {
                    for (var y = claim.Area.Top; y < claim.Area.Bottom; y++)
                    {
                        covercount[x, y]++;
                    }
                }
            }

            foreach (var z in covercount)
            {
                if (z > 1)
                    doubleCovered++;
            }

            return doubleCovered;
        }

        public int Puzzle2(string[] source)
        {
            var coversAnything = false;
            var claimsById = Parse(source);
            foreach (var claim in claimsById)
            {
                foreach (var other in claimsById)
                {
                    if (other.Key == claim.Key) 
                        continue; // don't check vs self

                    if (claim.Value.Covers(other.Value))
                    {
                        coversAnything = true;
                    }
                }

                if (!coversAnything)
                    return claim.Key;
                coversAnything = false; // reset for next iteration
            }

            return 0;
        }
    }

    public class Claim
    {
        public Rectangle Area { get; set; }
        public int Id { get; set; }

        public static Claim Parse(string source)
        {
            var clean = source.Replace(":", "").Replace("#", "").Replace(",", " ").Replace("x", " ").Replace("@", "");
            var subs = clean.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var ints = subs.Select(int.Parse).ToArray();
            return new Claim
            {
                Id = ints[0],
                Area = new Rectangle(ints[1], ints[2], ints[3], ints[4])
            };
        }

        public bool Covers(int x, int y)
        {
            return Area.Left < x && Area.Right > x &&
                   Area.Top < y && Area.Bottom > y;
        }

        public bool Covers(Claim other)
        {
            return other.Area.Left < Area.Right &&
                   other.Area.Right > Area.Left &&
                   other.Area.Top < Area.Bottom &&
                   other.Area.Bottom > Area.Top;
        }
    }
}
