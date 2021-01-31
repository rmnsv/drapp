using System.Collections.Generic;

namespace Drapp.Metronome
{
    public class Pattern
    {
        public string DisplayName { get; set; }
        internal readonly byte MaxSegmentation;
        internal readonly Dictionary<byte, BeatType> PatternInner;

        internal Pattern(string displayName, Dictionary<byte, BeatType> pattern, byte maxSegmentation)
        {
            DisplayName = displayName;
            MaxSegmentation = maxSegmentation;
            PatternInner = pattern;
        }
    }
}