namespace Drapp.Util
{
    public class TimeUtil
    {
        const int TicksInMinute = 600000000;

        internal static int BpmToTicks(int bpm) {
            return TicksInMinute / bpm;
        }
    }
}