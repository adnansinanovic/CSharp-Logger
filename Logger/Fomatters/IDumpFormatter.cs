using System;

namespace Logger
{
    public interface IDumpFormatter
    {
        Type FormatterType { get; }
        string Format(object value, int intendation = 0);
    }
}
