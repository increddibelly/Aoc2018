using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Aoc2018
{
    public class Day04
    {
        public EventList Parse(string[] input)
        {
            var result = new EventList(input);
            
            return result;
        }

        public Dictionary<int, List<Shift>> ToShifts(EventList list)
        {
            return list.Split().GroupBy(x => x.Guard).ToDictionary(x => x.Key, x => x.ToList());
        }

        public Dictionary<int, int> TotalSleepsPerMinute(IEnumerable<Shift> shifts)
        {
            var output = Shift.BlankHourSheet();
            for (var minute = 0; minute <= 59; minute++)
            {
                foreach (var shift in shifts)
                {
                    output[minute] += shift.SleepsPerMinute[minute];
                }
            }

            return output;
        }
    }
    
    public class Shift
    {
        public int Guard { get; protected set; }
        public Event[] Events { get; protected set; }
        public int SleepDuration { get; protected set; }
        public List<Period> Sleeps { get; protected set; }
        public Dictionary<int, int> SleepsPerMinute { get; set; } = BlankHourSheet();

        public Shift(Event[] events)
        {
            Events = events.OrderBy(x => x.Guard).ThenBy(x => x.Timestamp).ToArray();
            Sleeps = new List<Period>();
            Setup();
        }
        public static Dictionary<int, int> BlankHourSheet()
        {
            var d = new Dictionary<int, int>(60);
            for (var minute = 0; minute <= 59; minute++)
            {
                d.Add(minute, 0);
            }

            return d;
        }

        public string ToString()
        {
            return
                $"Guard# {Guard}, {Events?.Length ?? 0} Events, {Sleeps?.Count ?? 0} Sleeps for {SleepDuration} minutes";
        }

        private void Setup()
        {
            Guard = Events[0].Guard;
            SleepDuration = SleepDurationMinutes();
        }

        private int SleepDurationMinutes()
        {
            var sleepStart = DateTime.MinValue;
            var sleepEnd = DateTime.MaxValue;
            foreach (var t in Events)
            {
                switch (t.Type)
                {
                    case Event.EventType.SetGuard: break;
                    case Event.EventType.FallAsleep:
                        sleepStart = t.Timestamp;
                        break;
                    case Event.EventType.WakeUp:
                        sleepEnd = t.Timestamp;
                        var period = new Period {Start = sleepStart, End = sleepEnd};
                        Sleeps.Add(period);
                        for (var minute = period.Start.Minute; minute < period.End.Minute; minute++)
                        {
                            SleepsPerMinute[minute]++;
                        }
                        break;
                }
            }

            return Sleeps.Sum(x => x.Duration);
        }

        public int CountSleepsAt(int minute)
        {
            return Sleeps.Count(x => x.Start.TimeOfDay.Minutes <= minute && 
                                     x.End.TimeOfDay.Minutes   >  minute);
        }

        public bool SleepsAt(int minute)
        {
            return Sleeps.Any(x => x.Start.TimeOfDay.Minutes <= minute && 
                                   x.End.TimeOfDay.Minutes   >  minute);
        }
    }
    
    public class Event
    {
        public enum EventType : byte
        {
            SetGuard,
            WakeUp,
            FallAsleep
        };

        public DateTime Timestamp { get;set; }
        public int Guard { get; set; } = -1;
        public EventType Type { get; set; }

        public Event(string input)
        {
            Timestamp = ParseTime(input);
        }

        public static Event Parse(string input)
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

        private static Event ParseFallsAsleep(string input)
        {
            return new Event(input) {Type = EventType.FallAsleep};
        }

        private static Event ParseWakesUp(string input)
        {
            return new Event(input) {Type = EventType.WakeUp};
        }

        private static Event ParseGuard(string input)
        {
            var guardId = input.Split(' ')[3].Replace("#", "");
            return new Event (input)
            {
                Type = EventType.SetGuard,
                Guard = int.Parse(guardId)
            };
        }
    }

    public class EventList
    {
        private readonly List<Event> _events;

        public EventList(string[] source)
        {
            _events = source.Select(Event.Parse).OrderBy(x => x.Timestamp).ToList();

            var currentGuard = _events[0].Guard;
            foreach (var item in _events)
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
        }

        public List<Shift> Split()
        {
            var tempShift = new List<Event>();
            var shifts = new List<Shift>();
            foreach (var item in _events)
            {
                if (!tempShift.Any() || item.Type != Event.EventType.SetGuard)
                    tempShift.Add(item);
                else
                {
                    shifts.Add(new Shift(tempShift.ToArray()));
                    tempShift.Clear();
                }
            }

            return shifts.OrderBy(x => x.Guard).ToList();
        }

        private Shift FindShift(DateTime timestamp)
        {
            var middle = EventAtTime(timestamp);
            var start = FindShiftStart(middle);
            var end = FindShiftEnd(middle);

            return new Shift(_events
                .Where(x => x.Timestamp >= start.Timestamp)
                .Where(x => x.Timestamp <= end.Timestamp)
                .OrderBy(x => x.Timestamp)
                .ToArray());
        }

        /// <summary>
        /// first event (set guard) for this guard
        /// </summary>
        /// <returns></returns>
        public Event FindShiftStart(Event someEventInShift)
        {
            if (someEventInShift.Type == Event.EventType.SetGuard) return someEventInShift;
            return _events
                .Where(x => x.Type == Event.EventType.SetGuard)
                .Where(x => x.Timestamp < someEventInShift.Timestamp)
                .OrderByDescending(x => x.Timestamp)
                .Last();
        }

        /// <summary>
        /// last event for this guard
        /// </summary>
        /// <param name="someEventInShift"></param>
        /// <returns></returns>
        public Event FindShiftEnd(Event someEventInShift)
        {
            var next = _events.First(x => x.Timestamp > someEventInShift.Timestamp && x.Type != Event.EventType.SetGuard);
            return _events.Last(x => x.Timestamp < next.Timestamp);
        }

        public Event EventAtTime(DateTime timestamp)
        {
            Event current = null;

            foreach (var evt in _events)
            {
                if (evt.Timestamp < timestamp)
                    current = evt;
                if (evt.Timestamp >= timestamp)
                {
                    break;
                }
            }

            return current;
        }

        public Event this[int index] => _events[index];

        public IEnumerator<Event> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        public int Count => _events.Count;
    }

    public class Period
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Duration => (End-Start).Minutes;
    }

    public static class Extensions
    {
        public static int Characteristic(this double source)
        {
            return Convert.ToInt32(Math.Floor(source));
        }
    }
}
