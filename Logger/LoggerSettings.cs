using System;

namespace Logger
{
    public class LoggerSettings
    {
        public string FilePath { get; set; }
        public double MaxFileSizeMB { get; set; }

        /// <summary>
        /// Write each thread in different file.
        /// Thread id will be added to filepath.
        /// E.g if original path is c:\Log\mylog.log, and if thread with threadid = 2 is being logged, then filename will be c:\Log\myLog1.log
        /// </summary>
        public bool FilePerThread { get; set; }

        public LoggerSettings()
        {
            FilePath = $"{AppDomain.CurrentDomain.FriendlyName}_DELETE_ME.log";
            MaxFileSizeMB = 5;
            FilePerThread = false;
        }      

        internal int GetMaxSizeBytes()
        {
            return (int)ByteSize.FromMegaBytes(MaxFileSizeMB).Bytes;
        }
    }
}
