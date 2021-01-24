namespace Drapp.Metronome
{
    internal static class MetronomeUtil
    {
        internal static float BpmToMs(byte bpm)
        {
            return 60000 / (float) bpm;
        }
    }
}