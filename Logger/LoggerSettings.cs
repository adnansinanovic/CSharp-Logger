using System;

namespace Logger
{
    public class LoggerSettings
    {
        public string FilePath { get; set; }
        public double MaxFileSizeMB { get; set; }

        public LoggerSettings()
        {
            FilePath = $"{AppDomain.CurrentDomain.FriendlyName}_DELETE_ME.log";
            MaxFileSizeMB = 5;
        }      

        public int GetMaxSizeBytes()
        {
            return (int)ByteSize.FromMegaBytes(MaxFileSizeMB).Bytes;
        }
    }
}
