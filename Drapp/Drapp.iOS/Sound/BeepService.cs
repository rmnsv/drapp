using System;
using AVFoundation;
using Drapp.iOS.Sound;
using Drapp.Metronome;
using Drapp.Metronome.Sound;
using Drapp.Pattern;
using Foundation;
using Xamarin.Forms;

[assembly: Dependency(typeof(BeepService))]
namespace Drapp.iOS.Sound
{
    public class BeepService : IBeepService
    {

        private AVAudioPlayer _beepMediaPlayer;
        private AVAudioPlayer _accentMediaPlayer;
        private AVAudioPlayer _unaccentMediaPlayer;
        
        public event Action OnFinished;

        public BeepService()
        {
            _beepMediaPlayer = SetupMediaPlayer("sounds/metronome/beat.mp3");
            _accentMediaPlayer = SetupMediaPlayer("sounds/metronome/accent.mp3");
            _unaccentMediaPlayer = SetupMediaPlayer("sounds/metronome/unaccent.mp3");
        }
        
        public void Beep(EMetronomeBeep beepType)
        {
            switch (beepType)
            {
                case EMetronomeBeep.Beat:
                    if (_beepMediaPlayer.Playing)
                    {
                        _beepMediaPlayer?.Stop();
                    }
                    _beepMediaPlayer?.Play();
                    break;
                case EMetronomeBeep.Accented:
                    if (_accentMediaPlayer.Playing)
                    {
                        _accentMediaPlayer?.Stop();
                    }
                    _accentMediaPlayer?.Play();
                    break;
                case EMetronomeBeep.Unaccented:
                    if (_unaccentMediaPlayer.Playing)
                    {
                        _unaccentMediaPlayer?.Stop();
                    }
                    _unaccentMediaPlayer?.Play();
                    break;
            } 
        }
        
        private AVAudioPlayer SetupMediaPlayer(string beepFile)
        {
            AVAudioPlayer player = AVAudioPlayer.FromUrl(NSUrl.FromFilename(beepFile));
            player.FinishedPlaying += PlayerFinishedPlaying;
            return player;
        }

        private void PlayerFinishedPlaying(object sender, AVStatusEventArgs e)  
        {  
            OnFinished?.Invoke();  
        } 
    }
}