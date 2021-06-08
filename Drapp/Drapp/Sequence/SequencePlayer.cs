using System;
using System.Threading;
using System.Threading.Tasks;
using Drapp.Metronome.Model;
using Drapp.Native;
using Drapp.Util;

namespace Drapp.Sequence
{
    internal class SequencePlayer : ISequencePlayer
    {
        private bool _isPlaying;
        private int _mainInterval;
        
        private ISequence _sequence;
        private Task _playbackTask;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly PlaybackModel _playbackModel;
        
        private DrumMath _drumMath;
        
        public SequencePlayer(PlaybackModel model)
        {
            _playbackModel = model;
        }
        
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
        
        private void Reset()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            _playbackTask = SequenceFactory.CreatePlaybackTask(_sequence, cancellationToken);
        }
        
        public void Play()
        {
            if (_isPlaying || (_playbackTask != null && _playbackTask.Status == TaskStatus.Running))
            {
                return;
            }
            
            Reset();

            _playbackTask.Start();
            _isPlaying = true;

            Console.WriteLine("Playback started!");
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            if (!_isPlaying || _cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            _isPlaying = false;
            
            //TODO: reset visuals
            //OnVisualBeatOff?.Invoke();

            Console.WriteLine("Playback stopped!");
        }

        internal void SetBpm(int newBpm)
        {
            _mainInterval = CalculateMainInterval(newBpm);
            _sequence.UpdateMainInterval(_mainInterval);
        }
        
        internal void SetSequence(ISequence newSequence)
        {
            if (_isPlaying)
            {
                Stop();
                _sequence = newSequence;
                Play();
            }
            else
            {
                _sequence = newSequence;
            }
        }
    }
}