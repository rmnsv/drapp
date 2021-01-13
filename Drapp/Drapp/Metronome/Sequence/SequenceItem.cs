using System;

namespace Drapp.Metronome.Sequence
{
    public class SequenceItem
    {
        internal readonly int Time;
        internal readonly Action Action;

        internal SequenceItem(int time, Action action)
        {
            Time = time;
            Action = action;
        }
    }
}