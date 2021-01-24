using System;
using System.Collections.Generic;

namespace Drapp.Metronome.Sequence
{
    internal class SequenceEntity
    {
        private float _mainInterval;
        internal List<SequenceItem> Items;

        internal SequenceEntity(float mainInterval, List<SequenceItem> sequenceItems)
        {
            _mainInterval = mainInterval;
            Items = sequenceItems;
        }

        internal void UpdateMsInterval(float newInterval)
        {
            _mainInterval = newInterval;
            foreach (SequenceItem item in Items)
            {
                item.UpdateTime(_mainInterval);
            }
        }

        internal void InsertAt(int time, Action action)
        {
            if (time > _mainInterval || action == null)
            {
                return;
            }

            if (Items == null)
            {
            }

            if (time > 0)
            {
                
            }
            else
            {
            }
        }
    }
}