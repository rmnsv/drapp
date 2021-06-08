using Drapp.Sequence;

namespace Drapp.Metronome.Model
{
    public class PlaybackModel
    {
        public int Bpm { get; set; }
        public ISequence Sequence { get; set; }
    }
}