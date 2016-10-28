using System;

namespace Logger.Fomatters
{
    public class DateTimeFormatter : IDumpFormatter
    {        

        public DateTimeFormatter():this(string.Empty)
        {
        }

        public DateTimeFormatter(string dateFormat)
        {
            DateFormat = dateFormat;
        }

        public string DateFormat { get; set; }

        public Type FormatterType
        {
            get
            {
                return typeof(DateTime);
            }
        }

        public string Format(object value)
        {
            if (value == null)
                return "null";

            if (value is DateTime && !string.IsNullOrEmpty(DateFormat))
                return ((DateTime)value).ToString(DateFormat);


            return value.ToString();
        }
    }
}
