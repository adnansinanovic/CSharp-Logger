using System.IO;

namespace Logger
{
    public interface IObjectDumper
    {
        ObjectDumperSettings Settings { get; set; }
        void Dump(object obj, TextWriter textWriter);
        
    }
}
