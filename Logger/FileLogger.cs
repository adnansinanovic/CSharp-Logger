using Logger;
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
            //Settings.AddFormatter(new DateTimeFormatter());

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

        private static void WriteLine(string message)
        {
            producer.Add($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")} :: {message}{Environment.NewLine}{"-".PadRight(40, '-')} {Environment.NewLine}");
        }

        public static void WriteLine(object[] parameters, bool onItemOneRow = false)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object mesagge in parameters)
            {
                string dump = ObjectDumper.Write(mesagge);
                sb.Append($"{dump}");

                if (onItemOneRow)
                    sb.Append(Environment.NewLine);
            }

            WriteLine(sb.ToString());
        }

        public static void WriteLine(params object[] parameters)
        {
            if (parameters != null)
                WriteLine(parameters, true);
        }

        public static void Write(params object[] parameters)
        {
            if (parameters != null)
                WriteLine(parameters, false);
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
