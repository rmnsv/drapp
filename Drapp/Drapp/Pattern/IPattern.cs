using System.Collections.Generic;

namespace Drapp.Pattern
{
    public interface IPattern<T>
    {
        string DisplayName { get; set; }
        int MaxMeasurements { get; }
        IReadOnlyDictionary<byte, T> PatternInner { get; }
    }
}