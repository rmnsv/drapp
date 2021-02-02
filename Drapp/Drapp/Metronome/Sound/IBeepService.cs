using System;

namespace Drapp.Metronome.Sound
{
    public interface IBeepService
    {
        event Action OnFinished; 
        
        void Beep(BeatType beatType);
    }
}