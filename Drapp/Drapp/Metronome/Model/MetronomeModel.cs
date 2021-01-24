using Xamarin.Forms;

namespace Drapp.Metronome.Model
{
    public class MetronomeModel
    {
        public Color IndicatorColor { get; set; }
        public byte Bpm { get; set; }
        public Pattern Pattern { get; set; }
    }
}