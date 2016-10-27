using System;
using System.IO;
using System.Text;

namespace Logger
{
    public class FileLogger
    {
        private static ProducerConsumer<string> producer;
        public static LoggerSettings Settings { get; set; }

        static FileLogger()
        {
            Settings = new LoggerSettings();

            producer = new ProducerConsumer<string>($"{AppDomain.CurrentDomain.FriendlyName}.logger");
            producer.ItemProcessed += Producer_ItemProcessed;
        }

        private static void Producer_ItemProcessed(string message)
        {
            FileInfo fileInfo = new FileInfo(Settings.FilePath);
            if (fileInfo.Exists && fileInfo.Length > Settings.GetMaxSizeBytes())
                DeleteLogFile();                        

            using (FileStream file = new FileStream(fileInfo.FullName, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(message);
                }
            }
        }

        public static void DeleteLogFile()
        {
            FileInfo fileInfo = new FileInfo(Settings.FilePath);
            if (fileInfo.Exists)
            {
                File.Delete(fileInfo.FullName);
            }
        }

        public static void WriteLine(string message)
        {
            producer.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")} :: {message}{Environment.NewLine}{"-".PadRight(40, '-')} {Environment.NewLine}");
        }
    
        public static void WriteLine(string[] parameters, bool onItemOneRow = true)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string mesagge in parameters)
            {
                sb.Append($"{mesagge}");

                if (onItemOneRow)
                    sb.Append(Environment.NewLine);
            }

            WriteLine(sb.ToString());
        }

        public static void WriteLine(params string[] parameters)
        {
            if (parameters != null)
                WriteLine(parameters, true);
        }

        public static void WriteLine(object obj)
        {
            WriteLine(ObjectDumper.Write(obj));            
        }

        public static void WriteLine(Exception e)
        {            
            WriteLine(ExceptionHelper.CreateString(e));
        }    
    }
}
