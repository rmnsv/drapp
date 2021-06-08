namespace Drapp.Metronome.Sound
{
    public interface ITestMixer
    {
        void Init();

        void SetGain1(float gain1);

        void SetGain2(float gain2);

        void Mix(Note note1, Note note2);

        void Mix(Note note1, Drum drum2);

        void Mix(Drum drum1, Note note2);

        void Mix(Drum drum1, Drum drum2);
    }
}