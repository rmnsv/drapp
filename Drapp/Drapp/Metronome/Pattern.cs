using System.Collections.Generic;

namespace Drapp.Metronome
{
    public class Pattern
    {
        internal byte MaxSegmentation;
        internal Dictionary<byte, bool> PatternInner;

        public Pattern(Dictionary<byte, bool> pattern, byte maxSegmentation)
        {
            MaxSegmentation = maxSegmentation;
            PatternInner = pattern;
        }
    }
}