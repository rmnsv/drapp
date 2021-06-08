namespace Drapp.Sequence
{
    public interface ISequence
    {
        void UpdateMainInterval(int newInterval);
        bool ToNext();

        int GetCurrentIdleTime();

        void ExecuteCurrent();

        void ToBegin();
    }
}