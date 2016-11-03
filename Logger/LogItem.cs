using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logger
{
    public class LogItem
    {
        public LogItem()
        {
            Message = string.Empty;
            FileSuffix = string.Empty;
        }

        public string Message { get; set; }
        public string FileSuffix { get; set; }
    }
}
