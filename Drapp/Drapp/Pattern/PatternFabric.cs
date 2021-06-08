using System.Collections.Generic;
using Drapp.Metronome;

namespace Drapp.Pattern
{
    public class PatternFabric
    {
        internal List<MetronomePattern> CreateStandardPatterns()
        {
            return new List<MetronomePattern>()
            {
                Create4X4(),
                Create4X4_2(),
                Create4X3(),
            };
        }
        
        internal MetronomePattern Create4X4()
        {
            return new MetronomePattern("4x4", new Dictionary<byte, EMetronomeBeep>()
            {
                {0, EMetronomeBeep.Accented},
                {8, EMetronomeBeep.Unaccented},
                {16, EMetronomeBeep.Unaccented},
                {24, EMetronomeBeep.Unaccented}
            }, 32);
        }
        
        internal MetronomePattern Create4X4_2()
        {
            return new MetronomePattern("4x4_2", new Dictionary<byte, EMetronomeBeep>()
            {
                {0, EMetronomeBeep.Accented},
                {4, EMetronomeBeep.Beat},
                {8, EMetronomeBeep.Unaccented},
                {12, EMetronomeBeep.Beat},
                {16, EMetronomeBeep.Unaccented},
                {20, EMetronomeBeep.Beat},
                {24, EMetronomeBeep.Unaccented},
                {28, EMetronomeBeep.Beat},
            }, 32);
        }
        
        internal MetronomePattern Create4X3()
        {
            return new MetronomePattern("4x3",new Dictionary<byte, EMetronomeBeep>()
            {
                {0, EMetronomeBeep.Accented},
                {3, EMetronomeBeep.Beat},
                {6, EMetronomeBeep.Beat},
                {9, EMetronomeBeep.Unaccented},
                {12, EMetronomeBeep.Beat},
                {15, EMetronomeBeep.Beat},
                {18, EMetronomeBeep.Unaccented},
                {21, EMetronomeBeep.Beat},
                {24, EMetronomeBeep.Beat},
                {27, EMetronomeBeep.Unaccented},
                {30, EMetronomeBeep.Beat},
                {33, EMetronomeBeep.Beat},
            }, 36);
        }
    }
}