using System;
using System.Threading;
using System.Threading.Tasks;
using Drapp.Metronome.Model;
using Drapp.Metronome.Sequence;
using Drapp.Metronome.Sound;
using Drapp.Native;
using Drapp.Pattern;
using Drapp.Sequence;
using Drapp.Util;

namespace Drapp.Metronome
{
    internal class MetronomePlayer
    {
        private bool _isPlaying;
        private int _mainInterval;

        private MetronomeModel _metronomeModel;
        private IBeepService _beepService;
        
        private SequenceEntity _sequence;
        private Task _metronomeTask;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action<EMetronomeBeep> OnVisualBeatOn;
        public event Action OnVisualBeatOff;

        public MetronomePlayer(MetronomeModel model, IBeepService beepService)
        {
            _metronomeModel = model;
            _beepService = beepService;

            _beepService.OnFinished += () => OnVisualBeatOff?.Invoke();

            //_mainInterval = CalculateMainInterval(_metronomeModel.Bpm);

            //_sequence = BuildSequence(_mainInterval, _metronomeModel.Pattern);
        }

        //TODO: input
        private SequenceEntity BuildSequence(int mainInterval, MetronomePattern pattern)
        {
            SequenceEntity sequence = new SequenceEntity(mainInterval, pattern.MaxMeasurements);
            
            sequence.AddItem(0, () => Console.WriteLine("Main interval begin!"));

            foreach (var item in pattern.PatternInner)
            {
                sequence.AddItem(item.Key, () =>
                {
                    OnVisualBeatOn?.Invoke(item.Value);
                    _beepService?.Beep(item.Value);
                });
            }
            
            sequence.AddItem((byte) (pattern.MaxMeasurements - 1), () => Console.WriteLine("Main interval end!"));

            return sequence;
        }

        private void Reset()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            _metronomeTask = SequenceFactory.GetNewOLD(_sequence, cancellationToken);
        }

        private DrumMath _drumMath;

        private int CalculateMainInterval(int newBpm)
        {
            if (_drumMath == null)
            {
                _drumMath = new DrumMath();
            }
            
            int result = TimeUtil.BpmToTicks(newBpm) * 4;
            
            Console.WriteLine($"Drapp.Native says that bpm to ms is {result}!");
            return result; //TODO: think about quarter note constant
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

        internal void SetBpm(int newBpm)
        {
            _mainInterval = CalculateMainInterval(newBpm);
            _sequence.UpdateMainInterval(_mainInterval);
        }

        internal void SetPattern(MetronomePattern newPattern)
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