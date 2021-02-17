using System;
using System.Collections.Generic;
using System.Linq;

namespace Drapp.Metronome.Sequence
{
    internal class SequenceEntity
    {
        private double _mainInterval;
        private double _segmentTime;

        private SortedDictionary<byte, SequenceItem> _items;
        
        private byte _segmentation;

        internal List<SequenceItem> Items => _items.Values.ToList();

        public byte Segmentation => _segmentation;

        internal SequenceEntity(double mainInterval, byte segmentation)
        {
            _mainInterval = mainInterval;
            _segmentation = segmentation;
            
            UpdateSegmentTime();
        }

        internal void AddItem(byte segment, Action action)
        {
            if (_items == null)
            {
                _items = new SortedDictionary<byte, SequenceItem>();
            }
            
            if (_items.ContainsKey(segment))
            {
                _items[segment].AddAction(action);
                return;
            }
            _items.Add(segment, new SequenceItem(_mainInterval, _segmentation, action));
        }

        internal void PerformActionAt(byte segment)
        {
            if (_items.ContainsKey(segment))
            {
                _items[segment].Action?.Invoke();
            }
        }
        
        internal void UpdateMsInterval(double newInterval)
        {
            _mainInterval = newInterval;
            UpdateSegmentTime();
        }

        internal double GetSegmentTime()
        {
            return _segmentTime;
        }

        private void UpdateSegmentTime()
        {
            _segmentTime = _mainInterval / _segmentation;
        }
    }
}