using System.Collections.Generic;

namespace Drapp.Metronome
{
    public class PatternFabric
    {
        internal List<Pattern> CreateStandardPatterns()
        {
            return new List<Pattern>()
            {
                Create4X3(),
                Create4X4()
            };
        }
        
        internal Pattern Create4X4()
        {
            return new Pattern("4x4", new Dictionary<byte, BeatType>()
            {
                {0, BeatType.Accented},
                {8, BeatType.Unaccented},
                {16, BeatType.Unaccented},
                {24, BeatType.Unaccented}
            }, 32);
        }
        
        internal Pattern Create4X3()
        {
            return new Pattern("4x3",new Dictionary<byte, BeatType>()
            {
                {0, BeatType.Accented},
                {3, BeatType.Beat},
                {6, BeatType.Beat},
                {9, BeatType.Unaccented},
                {12, BeatType.Beat},
                {15, BeatType.Beat},
                {18, BeatType.Unaccented},
                {21, BeatType.Beat},
                {24, BeatType.Beat},
                {27, BeatType.Unaccented},
                {30, BeatType.Beat},
                {33, BeatType.Beat},
            }, 36);
        }
    }
}