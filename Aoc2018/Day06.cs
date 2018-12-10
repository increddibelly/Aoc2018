using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Aoc2018
{
    public class Day06
    {
        public Dictionary<int, int> Puzzle(string[] source)
        {
            var map = Map.Parse(source);

            var sizePerArea = new Dictionary<int, int>();
            foreach (var coordinate in map.MappedArea)
            {
                if (coordinate == null) continue; // no owner
                if (map.IsAtEdge(coordinate)) continue; // no infinite areas
                
                if (!sizePerArea.ContainsKey(coordinate.Id))
                    sizePerArea.Add(coordinate.Id, 0);
                sizePerArea[coordinate.Id]++;
            }
            return 
                sizePerArea.OrderBy(x => x.Value)
                           .ToDictionary(x => x.Key, x => x.Value);

        }

        public List<string> CreateMap(Map map)
        {
            List<string> output = new List<string>();
            
            var header = new StringBuilder();
            header.Append("     ");
            header.Append(" ");
            
            output.Add(header.ToString());

            for (var y = 0; y <= map.Height+1; y++)
            {
                var lineBuilder = new StringBuilder();
                for (var x = 0; x <= map.Width+1; x++)
                {
                    var closest = map.MappedArea[x, y];
                    if (closest == null)
                        lineBuilder.Append('-');
                    else
                    {
                        var letter = closest.Marker;
                        if (closest.X == x && closest.Y == y)
                        {
                            lineBuilder.Append((char) ((byte)letter));
                        } else if (map.IsAtEdge(closest))
                        {
                            lineBuilder.Append('~');
                        }
                        else
                        {
                            letter = (char)(letter + (byte)32); // lowercasify;
                            lineBuilder.Append((char) ((byte)letter));
                        }
                    }
                }
                output.Add(lineBuilder.ToString());
            }

            return output;
        }
    }

    public class Coordinate
    {
        private static int IdGenerator = 0;

        public int Id { get; }
        public int X { get;set; }
        public int Y { get; set; }
        public char Marker => (char) (byte) (64 + Id % 26);

        private Coordinate()
        {
            Id = ++IdGenerator;
        }

        public Coordinate(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public int ManhattanDistance(int x, int y)
        {
            return Math.Abs(x - X) + Math.Abs(y - Y);
        }

        public static Coordinate Parse(string input)
        {
            var elements = input.Replace(" ", "").Split(",");
            return new Coordinate
            {
                X = int.Parse(elements[0]),
                Y = int.Parse(elements[1])
            };
        }

        public new string ToString()
        {
            return $"Id {Id} @ {X},{Y}";
        }
    }

    public class Map
    {
        public Coordinate[] Markers { get; set; }
        public Coordinate[,] MappedArea { get; protected set; }

        public int Width { get; protected set; }
        public int Height {get; protected set; }

        public bool IsAtEdge(Coordinate coord)
        {
            if (MappedArea[0, coord.Y] == coord) return true;
            if (MappedArea[coord.X, 0] == coord) return true;
            if (MappedArea[Width, coord.Y] == coord) return true;
            if (MappedArea[coord.X, Height] == coord) return true;
            return false;
            var remainder = Markers;

            if (remainder.Any(x => x.X < coord.X))
            {
                // there's more markers to the left, check the ones to the right.
                remainder = remainder.Where(x => x.X > coord.X).ToArray();
            }
            else
            {
                // check the ones to the left
                remainder = remainder.Where(x => x.X < coord.X).ToArray();
            }

            if (remainder.Any(x => x.Y > coord.Y))
            {
                // there's more markers to the top, check the ones to the bottom
                remainder = remainder.Where(y => y.Y > coord.Y).ToArray();
            }
            else
            {
                // check the ones to the top
                remainder = remainder.Where(y => y.Y < coord.Y).ToArray();
            }

            return !remainder.Any();
        }

        protected Map(Coordinate[] source)
        {
            Markers = source;
            Width = Markers.Max(x => x.X);
            Height = Markers.Max(x => x.Y);
            CalculateClosest();
        }

        private void CalculateClosest()
        {
            MappedArea = new Coordinate[Width+2, Height+2];
            for (var x = 0; x < Width+2; x++)
            {
                for (var y = 0; y < Height+2; y++)
                {
                    MappedArea[x, y] = FindOwnerOf(x, y);
                }
            }
        }

        public static Map Parse(string[] input)
        {
            var source = input.Select(Coordinate.Parse).OrderBy(x => x.X).ThenBy(y => y.Y).ToArray();
            return new Map(source);
        }

        private Coordinate FindOwnerOf(int x, int y)
        {
            var distances = 
                Markers.ToDictionary(m => m, m => m.ManhattanDistance(x, y))
                       .OrderBy(m => m.Value)
                       .ToDictionary(m => m.Key, m => m.Value);

            Coordinate bestMatch = null;
            var bestDistance = -1;
            foreach (var item in distances)
            {
                if (bestDistance == -1)
                {
                    bestMatch = item.Key;
                    bestDistance = item.Value;
                    continue;
                }

                if (item.Value == bestDistance)
                    return null; // the list is sorted, so finding the same distance means no single distance is closest.

                if (item.Value > bestDistance)
                    return bestMatch; // next item is farther away, so previous item is our winner
            }

            return null;
        }

        public int SumOfDistances(int x, int y)
        {
            return Markers.Sum(mrk => mrk.ManhattanDistance(x, y));
        }

        public int[,] GetMarkerDistances()
        {
            var result = new int[Width+1,Height+1];
            for (var x = 0; x <= Width; x++)
            {
                for (var y = 0; y <= Height; y++)
                {
                    result[x, y] = SumOfDistances(x, y);
                }
            }

            return result;
        }

        public int GetMarkerDistancesBelow(int max)
        {
            var markerDistances = GetMarkerDistances();

            return markerDistances.Cast<int>().Count(coord => coord < max);
        }
    }
}
