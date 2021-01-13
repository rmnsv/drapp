using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Drapp.Metronome.Sequence;

namespace Drapp.Metronome
{
    public class Metronome
    {
        private bool _isPlaying;
        private int _mainInterval;

        private int _visualDimTime = 50;

        private SequenceEntity _sequence;
        private Task _metronomeTask;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action OnVisualAccentBegin;
        public event Action OnVisualAccentEnd;
        public event Action OnVisualClickBegin;
        public event Action OnVisualClickEnd;

        public Metronome(int mainInterval, Pattern pattern)
        {
            _mainInterval = mainInterval;

            _sequence = BuildSequence(_mainInterval, pattern);
        }

        //TODO: input
        private SequenceEntity BuildSequence(int mainInterval, Pattern pattern)
        {
            /* TODO: tired, think about it later
            Dictionary<byte, bool> patternInner = pattern.PatternInner;
            byte maxSegmentation = pattern.MaxSegmentation;

            int subIntervals = mainInterval / maxSegmentation;
            
            Dictionary<int, Action> resultDict = new Dictionary<int, Action>() {
                { 0, () => Console.WriteLine("Main interval begin!")},
                {mainInterval, () => Console.WriteLine("Main interval end!")}
            };

            foreach (KeyValuePair<byte,bool> pair in patternInner)
            {
                if (pair.Key == 0)
                {
                    if (pair.Value)
                    {
                        resultDict[0] += () => OnVisualAccentBegin?.Invoke();
                    }
                    else
                    {
                        resultDict[0] += () => OnVisualClickBegin?.Invoke();
                    }
                }
            }

            return resultDict;*/

            return new SequenceEntity(mainInterval, new List<SequenceItem>()
            {
                new SequenceItem(0, () =>
                {
                    Console.WriteLine("Main interval begin!");
                    OnVisualAccentBegin?.Invoke();
                }),
                new SequenceItem(_visualDimTime, () => OnVisualAccentEnd?.Invoke()),
                new SequenceItem(700, () => OnVisualClickBegin?.Invoke()),
                new SequenceItem(_visualDimTime, () => OnVisualClickEnd?.Invoke()),
                new SequenceItem(700, () => OnVisualClickBegin?.Invoke()),
                new SequenceItem(_visualDimTime, () => OnVisualClickEnd?.Invoke()),
                new SequenceItem(700, () => OnVisualClickBegin?.Invoke()),
                new SequenceItem(_visualDimTime, () => OnVisualClickEnd?.Invoke()),
                new SequenceItem(700, () => Console.WriteLine("Main interval end!")),
            });
        }

        private void Reset()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            _metronomeTask = SequenceTaskFactory.GetNew(_sequence, cancellationToken);
        }

        public void Play()
        {
            if (_isPlaying || (_metronomeTask != null && _metronomeTask.Status == TaskStatus.Running))
            {
                return;
            }
            
            Reset();

            _metronomeTask.Start();
            _isPlaying = true;

            Console.WriteLine("Metronome started!");
        }

        public void Stop()
        {
            if (!_isPlaying || _cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            _isPlaying = false;

            Console.WriteLine("Metronome stopped!");
        }
    }
}