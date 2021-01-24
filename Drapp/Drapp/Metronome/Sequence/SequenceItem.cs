using System;

namespace Drapp.Metronome.Sequence
{
    public class SequenceItem
    {
        private readonly byte _segments;
        private readonly byte _maxSegments;
        internal readonly Action Action;
        
        private float _time;

        internal SequenceItem(float mainInterval, byte segments, byte maxSegments, Action action)
        {
            _segments = segments;
            _maxSegments = maxSegments;
            Action = action;
            
            UpdateTime(mainInterval);
        }

        internal float GetTime()
        {
            return _time;
        }

        internal void UpdateTime(float mainInterval)
        {
            _time = mainInterval * _segments / _maxSegments;
        }
    }
}