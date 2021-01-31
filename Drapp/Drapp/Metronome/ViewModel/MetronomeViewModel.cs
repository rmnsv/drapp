using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public List<Pattern> Patterns { get; set; }

        private readonly MetronomeModel _metronomeModel;
        private readonly MetronomePlayer _metronomePlayer;

        public MetronomeViewModel(byte bpm)
        {
            PatternFabric patternFabric = new PatternFabric();
            Patterns = patternFabric.CreateStandardPatterns();
            
            _metronomeModel = new MetronomeModel()
            {
                Bpm = bpm,
                Pattern = Patterns.First()
            };

            _metronomePlayer = new MetronomePlayer(_metronomeModel);
            
            _metronomePlayer.OnVisualBeatOn += OnIndicatorOn;
            _metronomePlayer.OnVisualBeatOff += OnIndicatorOff;

            PlayCommand = new Command(Play);
            StopCommand = new Command(Stop);
            IncreaseBpmBy10Command = new Command(IncreaseBpmBy10);
            DecreaseBpmBy10Command = new Command(DecreaseBpmBy10);
        }

        private void OnIndicatorOn(BeatType beatType)
        {
            switch (beatType)
            {
                case BeatType.Accented:
                    AccentIndicatorColor = Color.Chartreuse;
                    break;
                case BeatType.Unaccented:
                    UnaccentIndicatorColor = Color.Aquamarine;
                    break;
            }
            BeatIndicatorColor = Color.Coral;
        }
        
        private void OnIndicatorOff()
        {
            AccentIndicatorColor = Color.Black;
            UnaccentIndicatorColor = Color.Black;
            BeatIndicatorColor = Color.Black;
        }

        public Color AccentIndicatorColor
        {
            get => _metronomeModel.AccentIndicatorColor;
            set
            {
                if (_metronomeModel.AccentIndicatorColor != value)
                {
                    _metronomeModel.AccentIndicatorColor = value;
                    OnPropertyChanged("AccentIndicatorColor");
                }
            }
        }
        
        public Color UnaccentIndicatorColor
        {
            get => _metronomeModel.UnaccentIndicatorColor;
            set
            {
                if (_metronomeModel.UnaccentIndicatorColor != value)
                {
                    _metronomeModel.UnaccentIndicatorColor = value;
                    OnPropertyChanged("UnaccentIndicatorColor");
                }
            }
        }
        
        public Color BeatIndicatorColor
        {
            get => _metronomeModel.BeatIndicatorColor;
            set
            {
                if (_metronomeModel.BeatIndicatorColor != value)
                {
                    _metronomeModel.BeatIndicatorColor = value;
                    OnPropertyChanged("BeatIndicatorColor");
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
                if (value != null && _metronomeModel.Pattern != value)
                {
                    _metronomeModel.Pattern = value;
                    _metronomePlayer?.SetPattern(value);
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

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}