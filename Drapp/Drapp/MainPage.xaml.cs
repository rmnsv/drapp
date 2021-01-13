using System;
using System.Collections.Generic;
using Drapp.Metronome;
using Xamarin.Forms;

namespace Drapp
{
    public partial class MainPage : ContentPage
    {
        private readonly Metronome.Metronome _metronome;

        public MainPage()
        {
            _metronome = new Metronome.Metronome(3000, new Pattern(new Dictionary<byte, bool>()
            {
                {0, true},
                {8, false},
                {16, false},
                {24, false}
            }, 32));
            
            _metronome.OnVisualAccentBegin += OnAccentBegin;
            _metronome.OnVisualAccentEnd += OnAccentEnd;
            _metronome.OnVisualClickBegin += OnClickBegin;
            _metronome.OnVisualClickEnd += OnClickEnd;

            InitializeComponent();
        }

        void OnPlayButton(object sender, EventArgs e)
        {
            _metronome?.Play();
        }

        void OnStopButton(object sender, EventArgs e)
        {
            _metronome?.Stop();
        }

        void OnClickBegin()
        {
            box.Color = Color.Aquamarine;
        }
        
        void OnClickEnd()
        {
            box.Color = Color.Bisque;
        }
        
        void OnAccentBegin()
        {
            box.Color = Color.Chartreuse;
        }
        
        void OnAccentEnd()
        {
            box.Color = Color.Bisque;
        }
    }
}
