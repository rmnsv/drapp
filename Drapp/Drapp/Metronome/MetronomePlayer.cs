using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Drapp.Metronome.Model;
using Drapp.Metronome.Sequence;

namespace Drapp.Metronome
{
    internal class MetronomePlayer
    {
        private bool _isPlaying;
        private float _mainInterval;

        private byte _visualDimTime = 2;

        private MetronomeModel _metronomeModel;
        
        private SequenceEntity _sequence;
        private Task _metronomeTask;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action OnVisualAccentBegin;
        public event Action OnVisualAccentEnd;
        public event Action OnVisualClickBegin;
        public event Action OnVisualClickEnd;

        public MetronomePlayer(MetronomeModel model)
        {
            _metronomeModel = model;

            _mainInterval = CalculateMainInterval(_metronomeModel.Bpm);

            _sequence = BuildSequence(_mainInterval, _metronomeModel.Pattern);
        }

        //TODO: input
        private SequenceEntity BuildSequence(float mainInterval, Pattern pattern)
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
                new SequenceItem(mainInterval, 0, pattern.MaxSegmentation,() =>
                {
                    Console.WriteLine("Main interval begin!");
                    OnVisualAccentBegin?.Invoke();
                }),
                new SequenceItem(mainInterval, _visualDimTime, pattern.MaxSegmentation, () => OnVisualAccentEnd?.Invoke()),
                new SequenceItem(mainInterval, 6, pattern.MaxSegmentation,() => OnVisualClickBegin?.Invoke()),
                new SequenceItem(mainInterval, _visualDimTime, pattern.MaxSegmentation,() => OnVisualClickEnd?.Invoke()),
                new SequenceItem(mainInterval, 6, pattern.MaxSegmentation,() => OnVisualClickBegin?.Invoke()),
                new SequenceItem(mainInterval, _visualDimTime, pattern.MaxSegmentation,() => OnVisualClickEnd?.Invoke()),
                new SequenceItem(mainInterval, 6, pattern.MaxSegmentation,() => OnVisualClickBegin?.Invoke()),
                new SequenceItem(mainInterval, _visualDimTime, pattern.MaxSegmentation,() => OnVisualClickEnd?.Invoke()),
                new SequenceItem(mainInterval, 6, pattern.MaxSegmentation,() => Console.WriteLine("Main interval end!")),
            });
        }

        private void Reset()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            _metronomeTask = SequenceTaskFactory.GetNew(_sequence, cancellationToken);
        }

        private float CalculateMainInterval(byte newBpm)
        {
            return MetronomeUtil.BpmToMs(newBpm) * 4; //TODO: think about quarter note constant
        }

        internal void Play()
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

        internal void Stop()
        {
            if (!_isPlaying || _cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            _isPlaying = false;

            Console.WriteLine("Metronome stopped!");
        }

        public void SetBpm(byte newBpm)
        {
            _mainInterval = MetronomeUtil.BpmToMs(newBpm) * 4;
            _sequence.UpdateMsInterval(_mainInterval);
        }
    }
}