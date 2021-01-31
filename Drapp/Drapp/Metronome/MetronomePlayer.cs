using System;
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

        public event Action<BeatType> OnVisualBeatOn;
        public event Action OnVisualBeatOff;

        public MetronomePlayer(MetronomeModel model)
        {
            _metronomeModel = model;

            _mainInterval = CalculateMainInterval(_metronomeModel.Bpm);

            _sequence = BuildSequence(_mainInterval, _metronomeModel.Pattern);
        }

        //TODO: input
        private SequenceEntity BuildSequence(float mainInterval, Pattern pattern)
        {
            SequenceEntity sequence = new SequenceEntity(mainInterval, pattern.MaxSegmentation);
            
            sequence.AddItem(0, () => Console.WriteLine("Main interval begin!"));

            foreach (var item in pattern.PatternInner)
            {
                sequence.AddItem(item.Key, () =>
                {
                    OnVisualBeatOn?.Invoke(item.Value);
                });
                sequence.AddItem((byte) (item.Key + 1), () => OnVisualBeatOff?.Invoke());
            }
            
            sequence.AddItem((byte) (pattern.MaxSegmentation - 1), () => Console.WriteLine("Main interval end!"));

            return sequence;
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
            OnVisualBeatOff?.Invoke();

            Console.WriteLine("Metronome stopped!");
        }

        internal void SetBpm(byte newBpm)
        {
            _mainInterval = CalculateMainInterval(newBpm);
            _sequence.UpdateMsInterval(_mainInterval);
        }

        internal void SetPattern(Pattern newPattern)
        {
            if (_isPlaying)
            {
                Stop();
                _sequence = BuildSequence(_mainInterval, newPattern);
                Play();
            }
            else
            {
                _sequence = BuildSequence(_mainInterval, newPattern);
            }
        } 
    }
}