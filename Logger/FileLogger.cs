using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

namespace Logger
{
    public class FileLogger
    {
        private static ProducerConsumer<LogItem> producer;
        public static LoggerSettings Settings { get; set; }

        public static ObjectDumperSettings DumperSettings { get { return _dumper.Settings; } set { _dumper.Settings = value; } }

        private static IObjectDumper _dumper;

        static FileLogger()
        {
            SetDumpMethod(ObjectDumpMethod.Traverse);

            Settings = new LoggerSettings();            

            producer = new ProducerConsumer<LogItem>();
            producer.ItemProcessed += Producer_ItemProcessed;
        }

        public static void SetDumpMethod(ObjectDumpMethod method)
        {
            switch (method)
            {
                case ObjectDumpMethod.Recursive:
                    _dumper = new RecursiveObjectDumper();                    
                    break;
                case ObjectDumpMethod.Traverse:                    
                    _dumper = new TraversalObjectDumper();
                    break;
                default:
                    throw new InvalidEnumArgumentException(method.ToString());
            }                        
        }

        private static void Producer_ItemProcessed(LogItem logItem)
        {
            FileInfo tempFileInfo = new FileInfo(Settings.FilePath);
            string path = $"{Settings.FilePath.Replace(tempFileInfo.Extension, logItem.FileSuffix)}{tempFileInfo.Extension}";

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists && fileInfo.Length > Settings.GetMaxSizeBytes())
                DeleteLogFile();

            using (FileStream file = new FileStream(fileInfo.FullName, FileMode.Append, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
                {
                    writer.Write(logItem.Message);
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

        private static void FinalWrite(string message, bool multiline)
        {
            LogItem item = new LogItem();

            StringBuilder sb = new StringBuilder();
            sb.Append($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}");            
            sb.Append( multiline ? $"{Environment.NewLine}" : " :: ");
            sb.AppendLine(message);
            sb.Append($"{"-".PadRight(40, '-')}{Environment.NewLine}");
            
            item.Message = sb.ToString();

            item.FileSuffix = Settings.FilePerThread ? Thread.CurrentThread.ManagedThreadId.ToString() : string.Empty;

            producer.Add(item);
        }

        //public static void WriteLine(string message)
        //{
        //    FinalWrite(message, false);
        //}

        public static void WriteLine(object[] parameters, bool oneItemOneRow = false)
        {
            using (TextWriter textWriter = new StringWriter())
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    _dumper.Dump(parameters[i], textWriter);

                    if (oneItemOneRow && i + 1 < parameters.Length)
                        textWriter.WriteLine();
                }

                bool isMultiline = oneItemOneRow && (parameters.Length > 1 || (parameters.Length == 1 && parameters[0] is IEnumerable));

                FinalWrite(textWriter.ToString(), isMultiline);
            }
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
            using (TextWriter textWriter = new StringWriter())
            {
                _dumper.Dump(obj, textWriter);

                bool isMultiline = (obj != null) && !(obj is ValueType) && !(obj is string);                            

                FinalWrite(textWriter.ToString(), isMultiline);
            }
        }

        public static void WriteLine(Exception e, string prefix = "")
        {
            FinalWrite($"{prefix}{Environment.NewLine}{ExceptionHelper.CreateString(e)}", true);
        }
    }
}
