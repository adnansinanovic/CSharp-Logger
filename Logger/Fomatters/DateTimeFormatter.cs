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

        public string Format(object value, int intendation)
        {
            if (value == null)
                return "null";

            if (value is DateTime && !string.IsNullOrEmpty(DateFormat))
                return $"{string.Empty.PadRight(intendation, '\t')}{((DateTime)value).ToString(DateFormat)}";


            return $"{string.Empty.PadRight(intendation, '\t')}{value.ToString()}";
        }
    }
}
