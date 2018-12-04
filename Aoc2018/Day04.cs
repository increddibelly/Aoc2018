using System;
using System.Globalization;
using System.Linq;

namespace Aoc2018
{
    public class Day04
    {
        public Log[] Parse(string[] input)
        {
            var result = input.Select(Log.Parse).OrderBy(x=> x.Timestamp).ToArray();
            var currentGuard = result[0].Guard;
            foreach (var item in result)
            {
                if (item.Guard > 0)
                {
                    currentGuard = item.Guard;
                }
                else
                {
                    item.Guard = currentGuard;
                }
            }
            return result;
        }
    }

    public class Log
    {
        public DateTime Timestamp { get;set; }
        public int Guard { get; set; } = -1;
        public bool WakeUp = false;
        public bool FallAsleep = false;

        public Log(string input)
        {
            Timestamp = ParseTime(input);
        }

        public static Log Parse(string input)
        {
            if (input.Contains("Guard")) return ParseGuard(input);
            if (input.Contains("wakes")) return ParseWakesUp(input);
            if (input.Contains("falls")) return ParseFallsAsleep(input);
            return null;
        }

        private static DateTime ParseTime(string input)
        {
            input = input.Replace("1518-", "2018-");
            return DateTime.ParseExact(input.Substring(1, 16), "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture);
        }

        private static Log ParseFallsAsleep(string input)
        {
            return new Log(input) {FallAsleep = true};
        }

        private static Log ParseWakesUp(string input)
        {
            return new Log(input) {WakeUp = true};
        }

        private static Log ParseGuard(string input)
        {
            var guardId = input.Split(' ')[3].Replace("#", "");
            return new Log (input)
            {
                Guard = int.Parse(guardId)
            };
        }
    }
}
