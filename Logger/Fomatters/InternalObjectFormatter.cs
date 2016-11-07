using System.IO;

namespace Logger.Fomatters
{
    internal class InternalObjectFormatter
    {
        public void NewLine(TextWriter tw)
        {
            tw.WriteLine();
        }

        public void WriteName(NameContainer name, int textTabs, TextWriter tw, bool fullName)
        {
            WriteText($"{name.GetName(fullName)} = ", textTabs, tw);            
        }

        public void WriteText(string text, int tabs, TextWriter tw)
        {
            tw.Write(string.Empty.PadRight(tabs, '\t'));
            tw.Write(text);
        }

        public void WriteValue(object value, TextWriter tw)
        {
            bool isString = (value is string);

            string v = value == null ? "null" : isString ? $"\"{value.ToString()}\"" : value.ToString();

            WriteText(v, 0, tw);
        }
    }
}
