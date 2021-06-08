using System;
using Drapp.Metronome.Sound;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drapp.Metronome.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestSoundGeneratorView : ContentView
    {
        private readonly ITestGenerator _generator;
        public TestSoundGeneratorView()
        {
            _generator = DependencyService.Get<ITestGenerator>();
            _generator.Init();
            InitializeComponent();
        }

        public void OnC5(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.C5);
        }

        public void OnD5(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.D5);
        }
        
        public void OnE5(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.E5);
        }
        
        public void OnF5(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.F5);
        }
        
        public void OnG5(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.G5);
        }
        
        public void OnA5(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.A5);
        }
        
        public void OnB5(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.B5);
        }
        
        public void OnC6(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.C6);
        }
        
        public void OnD6(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.D6);
        }
        
        public void OnE6(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.E6);
        }
        
        public void OnF6(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.F6);
        }
        
        public void OnG6(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.G6);
        }
        
        public void OnA6(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.A6);
        }
        
        public void OnB6(object sender, EventArgs eventArgs)
        {
            _generator.PlayNote(Note.B6);
        }
        
        public void OnKick(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.Kick);
        }
        
        public void OnSnare(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.Snare);
        }
        
        public void OnTom1(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.Tom1);
        }
        
        public void OnTom2(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.Tom2);
        }
        
        public void OnFloorTom(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.FloorTom);
        }
        
        public void OnHiHat(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.HiHat);
        }
        
        public void OnRide(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.Ride);
        }
        
        public void OnCrash1(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.Crash1);
        }
        
        public void OnCrash2(object sender, EventArgs eventArgs)
        {
            _generator.PlayDrum(Drum.Crash2);
        }
        
        public void OnMix1(object sender, EventArgs eventArgs)
        {
            _generator.PlayMix1();
        }
        
        public void OnMix2(object sender, EventArgs eventArgs)
        {
            _generator.PlayMix2();
        }
        
        public void OnMix3(object sender, EventArgs eventArgs)
        {
            _generator.PlayMix3();
        }
    }
}