using System.Collections.Generic;
using Drapp.Metronome;

namespace Drapp.Pattern
{
    public class MetronomePattern : IPattern<EMetronomeBeep>
    {
        public string DisplayName { get; set; }
        public int MaxMeasurements { get; }
        public IReadOnlyDictionary<byte, EMetronomeBeep> PatternInner { get; }

        internal MetronomePattern(string displayName, Dictionary<byte, EMetronomeBeep> pattern, byte maxMeasurements)
        {
            DisplayName = displayName;
            MaxMeasurements = maxMeasurements;
            PatternInner = pattern;
        }
    }
}