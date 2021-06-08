using System.Collections.Generic;

namespace Drapp.Pattern
{
    public class DrumPattern : IPattern<EDrumNote>
    {
        public string DisplayName { get; set; }
        public int MaxMeasurements { get; }
        public IReadOnlyDictionary<byte, EDrumNote> PatternInner { get; }
    }
}