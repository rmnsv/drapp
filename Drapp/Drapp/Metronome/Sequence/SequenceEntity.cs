using System;
using System.Collections.Generic;
using System.Linq;

namespace Drapp.Metronome.Sequence
{
    internal class SequenceEntity
    {
        private int _mainInterval;
        private int _measurementTime;

        private SortedDictionary<byte, SequenceItem> _items;
        
        private int _measurementsCount;

        internal List<SequenceItem> Items => _items.Values.ToList();

        public int MeasurementsCount => _measurementsCount;

        internal SequenceEntity(int mainInterval, int measurementsCount)
        {
            _mainInterval = mainInterval;
            _measurementsCount = measurementsCount;
            
            UpdateMeasurementTime();
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
            _items.Add(segment, new SequenceItem(_mainInterval, _measurementsCount, action));
        }

        internal void PerformActionAt(byte segment)
        {
            if (_items.ContainsKey(segment))
            {
                _items[segment].Action?.Invoke();
            }
        }
        
        internal void UpdateMainInterval(int newInterval)
        {
            _mainInterval = newInterval;
            UpdateMeasurementTime();
        }

        internal int GetMeasurementTime()
        {
            return _measurementTime;
        }

        private void UpdateMeasurementTime()
        {
            _measurementTime = _mainInterval / _measurementsCount;
        }
    }
}