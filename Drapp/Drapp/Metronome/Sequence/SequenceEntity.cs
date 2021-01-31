using System;
using System.Collections.Generic;
using System.Linq;

namespace Drapp.Metronome.Sequence
{
    internal class SequenceEntity
    {
        private float _mainInterval;
        private float _segmentTime;

        private SortedDictionary<byte, SequenceItem> _items;
        
        private byte _segmentation;

        internal List<SequenceItem> Items => _items.Values.ToList();

        public byte Segmentation => _segmentation;

        internal SequenceEntity(float mainInterval, byte segmentation)
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
        
        internal void UpdateMsInterval(float newInterval)
        {
            _mainInterval = newInterval;
            UpdateSegmentTime();
        }

        internal float GetSegmentTime()
        {
            return _segmentTime;
        }

        private void UpdateSegmentTime()
        {
            _segmentTime = _mainInterval / _segmentation;
        }
    }
}