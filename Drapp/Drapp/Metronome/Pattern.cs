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

        public static Pattern CreateDefault()
        {
            return new Pattern(new Dictionary<byte, bool>()
            {
                {0, true},
                {8, false},
                {16, false},
                {24, false}
            }, 32);
        }
    }
}