using System;
using Drapp.Pattern;

namespace Drapp.Metronome.Sound
{
    public interface IBeepService
    {
        event Action OnFinished; 
        
        void Beep(EMetronomeBeep beepType);
    }
}