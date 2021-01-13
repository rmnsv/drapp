using System;
using System.Collections.Generic;

namespace Drapp.Metronome.Sequence
{
    internal class SequenceEntity
    {
        private int _mainInterval;
        internal List<SequenceItem> Items;

        internal SequenceEntity(int mainInterval, List<SequenceItem> sequenceItems)
        {
            _mainInterval = mainInterval;
            Items = sequenceItems;
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