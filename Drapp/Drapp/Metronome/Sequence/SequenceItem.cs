using System;

namespace Drapp.Metronome.Sequence
{
    internal class SequenceItem
    {
        private readonly byte _segmentation;
        
        private Action _action;
        
        internal Action Action => _action;

        internal SequenceItem(float mainInterval, byte segmentation, Action action)
        {
            _segmentation = segmentation;
            _action = action;
        }

        internal void AddAction(Action action)
        {
            _action += action;
        }
    }
}