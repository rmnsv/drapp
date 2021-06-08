/*using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Drapp.Metronome.Model;
using Drapp.Metronome.Sound;
using Drapp.Pattern;
using Xamarin.Forms;

namespace Drapp.Metronome.ViewModel
{
    public class MetronomeViewModel : INotifyPropertyChanged
    {
        public List<MetronomePattern> Patterns { get; set; }

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

            _metronomePlayer = new MetronomePlayer(_metronomeModel, DependencyService.Get<IBeepService>());
            
            _metronomePlayer.OnVisualBeatOn += OnIndicatorOn;
            _metronomePlayer.OnVisualBeatOff += OnIndicatorOff;

            PlayCommand = new Command(Play);
            StopCommand = new Command(Stop);
            IncreaseBpmBy1Command = new Command(() => ChangeBpm(1));
            IncreaseBpmBy10Command = new Command(() => ChangeBpm(10));
            DecreaseBpmBy1Command = new Command(() => ChangeBpm(-1));
            DecreaseBpmBy10Command = new Command(() => ChangeBpm(-10));
        }

        private void OnIndicatorOn(EMetronomeBeep beepType)
        {
            switch (beepType)
            {
                case EMetronomeBeep.Accented:
                    AccentIndicatorColor = Color.Chartreuse;
                    break;
                case EMetronomeBeep.Unaccented:
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
        

        
        public MetronomePattern Pattern
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



        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}*/