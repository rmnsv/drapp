namespace Drapp.Metronome.Sound
{
    public interface ITestGenerator
    {
        void Init();

        void PlayNote(Note note);

        void PlayDrum(Drum drum);

        void PlayMix1();

        void PlayMix2();

        void PlayMix3();
    }
}