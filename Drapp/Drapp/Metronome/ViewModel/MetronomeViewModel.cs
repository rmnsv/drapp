using System.ComponentModel;
using System.Windows.Input;
using Drapp.Metronome.Model;
using Xamarin.Forms;

namespace Drapp.Metronome.ViewModel
{
    public class MetronomeViewModel : INotifyPropertyChanged
    {
        private const byte MIN_BPM = 10;
        private const byte MAX_BPM = 240;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand PlayCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand IncreaseBpmBy10Command { get; }
        public ICommand DecreaseBpmBy10Command { get; }

        private readonly MetronomeModel _metronomeModel;
        private readonly MetronomePlayer _metronomePlayer;

        public MetronomeViewModel(Color indicatorColor, byte bpm, Pattern pattern)
        {
            _metronomeModel = new MetronomeModel()
            {
                IndicatorColor = indicatorColor,
                Bpm = bpm,
                Pattern = pattern
            };

            _metronomePlayer = new MetronomePlayer(_metronomeModel);
            
            _metronomePlayer.OnVisualAccentBegin += OnAccentBegin;
            _metronomePlayer.OnVisualAccentEnd += OnAccentEnd;
            _metronomePlayer.OnVisualClickBegin += OnClickBegin;
            _metronomePlayer.OnVisualClickEnd += OnClickEnd;

            PlayCommand = new Command(Play);
            StopCommand = new Command(Stop);
            IncreaseBpmBy10Command = new Command(IncreaseBpmBy10);
            DecreaseBpmBy10Command = new Command(DecreaseBpmBy10);
        }
        
        public Color IndicatorColor
        {
            get => _metronomeModel.IndicatorColor;
            set
            {
                if (_metronomeModel.IndicatorColor != value)
                {
                    _metronomeModel.IndicatorColor = value;
                    OnPropertyChanged("IndicatorColor");
                }
            }
        }
        
        public byte Bpm
        {
            get => _metronomeModel.Bpm;
            set
            {
                if (_metronomeModel.Bpm != value && value >= MIN_BPM && value <= MAX_BPM)
                {
                    _metronomeModel.Bpm = value;
                    _metronomePlayer?.SetBpm(value);
                    OnPropertyChanged("Bpm");
                }
            }
        }
        
        public Pattern Pattern
        {
            get => _metronomeModel.Pattern;
            set
            {
                if (_metronomeModel.Pattern != value)
                {
                    _metronomeModel.Pattern = value;
                    OnPropertyChanged("Pattern");
                }
            }
        }

        private void Play()
        {
            _metronomePlayer?.Play();
        }

        private void Stop()
        {
            _metronomePlayer?.Stop();
        }

        private void IncreaseBpmBy10()
        {
            Bpm += 10;
        }

        private void DecreaseBpmBy10()
        {
            Bpm -= 10;
        }

        private void OnClickBegin()
        {
            IndicatorColor = Color.Aquamarine;
        }
        
        private void OnClickEnd()
        {
            IndicatorColor = Color.Bisque;
        }
        
        private void OnAccentBegin()
        {
            IndicatorColor = Color.Indigo;
        }
        
        private void OnAccentEnd()
        {
            IndicatorColor = Color.Bisque;
        }

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}