using System.ComponentModel;
using System.Windows.Input;
using Drapp.Metronome.Model;
using Drapp.Sequence;
using Xamarin.Forms;

namespace Drapp.Metronome.ViewModel
{
    public class PlaybackViewModel : INotifyPropertyChanged
    {
        private const byte MIN_BPM = 30;
        private const byte MAX_BPM = 250;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ICommand PlayCommand { get; }
        public ICommand StopCommand { get; }
        
        public ICommand IncreaseBpmBy1Command { get; }
        public ICommand IncreaseBpmBy10Command { get; }
        public ICommand DecreaseBpmBy1Command { get; }
        public ICommand DecreaseBpmBy10Command { get; }

        private readonly PlaybackModel _playbackModel;
        private readonly SequencePlayer _player;
        
        public int Bpm
        {
            get => _playbackModel.Bpm;
            set
            {
                if (_playbackModel.Bpm != value)
                {
                    if (value >= MAX_BPM)
                    {
                        _playbackModel.Bpm = MAX_BPM;
                    } else if (value <= MIN_BPM)
                    {
                        _playbackModel.Bpm = MIN_BPM;
                    }
                    else
                    {
                        _playbackModel.Bpm = value;
                    }
                    _player?.SetBpm(_playbackModel.Bpm);
                    OnPropertyChanged("Bpm");
                }
            }
        }
        
        public PlaybackViewModel(byte bpm)
        {
            _playbackModel = new PlaybackModel()
            {
                Bpm = bpm,
                Sequence = null //TODO: safely
            };

            _player = new SequencePlayer(_playbackModel);
            
            /* TODO: subscribe to visuals
            _metronomePlayer.OnVisualBeatOn += OnIndicatorOn;
            _metronomePlayer.OnVisualBeatOff += OnIndicatorOff;
            */

            PlayCommand = new Command(Play);
            StopCommand = new Command(Stop);
            IncreaseBpmBy1Command = new Command(() => ChangeBpm(1));
            IncreaseBpmBy10Command = new Command(() => ChangeBpm(10));
            DecreaseBpmBy1Command = new Command(() => ChangeBpm(-1));
            DecreaseBpmBy10Command = new Command(() => ChangeBpm(-10));
        }
        
        private void Play()
        {
            _player?.Play();
        }

        private void Stop()
        {
            _player?.Stop();
        }

        private void ChangeBpm(int delta)
        {
            Bpm += delta;
        }
        
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}