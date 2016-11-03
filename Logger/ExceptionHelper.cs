using System;
using System.Text;

namespace Logger
{
    public class ExceptionHelper
    {
        public static string CreateString(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            while (e != null)
            {
                sb.AppendLine(e.Message);
                sb.AppendLine(e.StackTrace);
                sb.AppendLine(e.Source);
                e = e.InnerException;

                if (e != null)
                    sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
