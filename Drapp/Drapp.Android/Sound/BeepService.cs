using System;
using System.Collections.Generic;
using Android.Content;
using Android.Content.Res;
using Android.Media;
using Android.Util;
using Drapp.Android.Sound;
using Drapp.Metronome;
using Drapp.Metronome.Sound;
using Drapp.Pattern;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(BeepService))]
namespace Drapp.Android.Sound
{
    public class BeepService : IBeepService
    {
        private SoundPool _pool;

        private Dictionary<EMetronomeBeep, int> _beepIds = new Dictionary<EMetronomeBeep, int>()
        {
            {EMetronomeBeep.Beat, 0},
            {EMetronomeBeep.Accented, 0},
            {EMetronomeBeep.Unaccented, 0}
        };
        
        
        //TODO: remove if unusable
        public event Action OnFinished;

        public BeepService()
        {
            SoundPool.Builder builder = new SoundPool.Builder();

            builder.SetMaxStreams(3);
            
            AudioAttributes attributes = new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Media)
                ?.SetContentType(AudioContentType.Sonification)
                ?.Build();

            builder.SetAudioAttributes(attributes);

            _pool = builder.Build();

            if (_pool != null)
            {
                AssetFileDescriptor beepAfd = null;
                AssetFileDescriptor accentAfd = null;
                AssetFileDescriptor unaccentAfd = null;

                try
                {
                    Context context = Platform.AppContext;
                    
                    if (context?.Assets == null)
                    {
                        throw new AndroidException();
                    }

                    beepAfd = context.Assets.OpenFd("sounds/metronome/beat.mp3");
                    accentAfd = context.Assets.OpenFd("sounds/metronome/accent.mp3");
                    unaccentAfd = context.Assets.OpenFd("sounds/metronome/unaccent.mp3");
                }  
                catch (Exception ex)  
                {  
                    Console.WriteLine("Error openfd: " + ex);  
                }

                if (beepAfd != null && accentAfd != null && unaccentAfd != null)
                {
                    _beepIds[EMetronomeBeep.Beat] = _pool.Load(beepAfd, 1);
                    _beepIds[EMetronomeBeep.Accented] = _pool.Load(accentAfd, 1);
                    _beepIds[EMetronomeBeep.Unaccented] = _pool.Load(unaccentAfd, 1);
                }
            }
        }
        
        public void Beep(EMetronomeBeep beepType)
        {
            //OnFinished?.Invoke(); 
            _pool.Play(_beepIds[beepType], 1.0f, 1.0f, 1, 0, 1);
        }
        
        
        private void PlayerFinishedPlaying(object sender, EventArgs e)  
        {  
            OnFinished?.Invoke();  
        }
    }
}