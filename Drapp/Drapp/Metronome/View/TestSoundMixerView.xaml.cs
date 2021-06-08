using System;
using System.Collections.Generic;
using System.Linq;
using Drapp.Metronome.Sound;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drapp.Metronome.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestSoundMixerView : ContentView
    {
        private readonly ITestMixer _mixer;
        
        public TestSoundMixerView()
        {
            _mixer = DependencyService.Get<ITestMixer>();
            _mixer.Init();
            
            InitializeComponent();

            List<string> items = Enum.GetNames(typeof(Note)).ToList();

            items.AddRange(Enum.GetNames(typeof(Drum)).ToList());
            
            Sound1.ItemsSource = items;
            Sound1.SelectedIndex = 0;
            
            Sound2.ItemsSource = items;
            Sound2.SelectedIndex = 0;
        }

        void OnGain1Change(object sender, ValueChangedEventArgs e)
        {
            _mixer.SetGain1((float) e.NewValue);
        }
        
        void OnGain2Change(object sender, ValueChangedEventArgs e)
        {
            _mixer.SetGain2((float) e.NewValue);
        }

        void OnMixButton(object sender, EventArgs eventArgs)
        {
            string item1 = (string) Sound1.SelectedItem;
            string item2 = (string) Sound2.SelectedItem;
            
            bool item1IsNote = item1.Length == 2;
            bool item2IsNote = item2.Length == 2;

            if (item1IsNote && item2IsNote)
            {
                _mixer.Mix((Note) Enum.Parse(typeof(Note), item1), (Note) Enum.Parse(typeof(Note), item2));
            }
            else if (item1IsNote)
            {
                _mixer.Mix((Note) Enum.Parse(typeof(Note), item1), (Drum) Enum.Parse(typeof(Drum), item2));
            }
            else if (item2IsNote)
            {
                _mixer.Mix((Drum) Enum.Parse(typeof(Drum), item1), (Note) Enum.Parse(typeof(Note), item2));
            }
            else
            {
                _mixer.Mix((Drum) Enum.Parse(typeof(Drum), item1), (Drum) Enum.Parse(typeof(Drum), item2));
            }
        }
    }
}