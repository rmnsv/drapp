using System;
using System.Collections.Generic;
using Android.Content.Res;
using Android.Media;
using Drapp.Android.Sound;
using Drapp.Metronome;
using Drapp.Metronome.Sound;
using Xamarin.Forms;

[assembly: Dependency(typeof(BeepService))]
namespace Drapp.Android.Sound
{
    public class BeepService : IBeepService
    {
        private MediaPlayer _accentMediaPlayer;  
        private MediaPlayer _unaccentMediaPlayer; 
        private MediaPlayer _beepMediaPlayer;
        
        private Dictionary<BeatType, bool> _playerReadiness = new Dictionary<BeatType, bool>()
        {
            {BeatType.Beat, false},
            {BeatType.Accented, false},
            {BeatType.Unaccented, false}
        };
        
        public event Action OnFinished;

        public BeepService()
        {
            _beepMediaPlayer = SetupMediaPlayer(BeatType.Beat, "sounds/metronome/beat.mp3");
            _accentMediaPlayer = SetupMediaPlayer(BeatType.Accented, "sounds/metronome/accent.mp3");
            _unaccentMediaPlayer = SetupMediaPlayer(BeatType.Unaccented, "sounds/metronome/unaccent.mp3");
        }
        
        public void Beep(BeatType beatType)
        {
            if (!_playerReadiness[beatType])
            {
                return;
            }
            
            switch (beatType)
            {
                case BeatType.Beat:
                    if (_beepMediaPlayer.IsPlaying)
                    {
                        _beepMediaPlayer?.Stop();
                    }
                    _beepMediaPlayer?.Start();
                    break;
                case BeatType.Accented:
                    if (_accentMediaPlayer.IsPlaying)
                    {
                        _accentMediaPlayer?.Stop();
                    }
                    _accentMediaPlayer?.Start();
                    break;
                case BeatType.Unaccented:
                    if (_unaccentMediaPlayer.IsPlaying)
                    {
                        _unaccentMediaPlayer?.Stop();
                    }
                    _unaccentMediaPlayer?.Start();
                    break;
            }
        }

        private MediaPlayer SetupMediaPlayer(BeatType beatType, string beepFile)
        {
            AssetFileDescriptor afd = null;

            try  
            {
                afd = Forms.Context.Assets.OpenFd(beepFile);  
            }  
            catch (Exception ex)  
            {  
                Console.WriteLine("Error openfd: " + ex);  
            }  
            
            if (afd != null)  
            {  
                System.Diagnostics.Debug.WriteLine("Length " + afd.Length);  
                
                MediaPlayer player = new MediaPlayer();
                player.Prepared += (sender, args) =>  
                {
                    player.Completion += PlayerFinishedPlaying;
                    _playerReadiness[beatType] = true;
                };

                player.Reset();  
                player.SetVolume(1.0f, 1.0f);  
  
                player.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);  
                player.PrepareAsync();
                return player;
            }  
            
            return null;
        }
        
        private void PlayerFinishedPlaying(object sender, EventArgs e)  
        {  
            OnFinished?.Invoke();  
        } 
    }
}