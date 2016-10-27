using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Logger
{
    public static class ObjectDumper
    {

        public static ObjectDumperSettings Settings;

        static ObjectDumper()
        {
            Settings = new ObjectDumperSettings();
        }

        public static string Write(object obj)
        {
            string result = string.Empty;
            using (TextWriter tw = new StringWriter())
            {
                Dump(obj, tw, 0);
                result = tw.ToString();

                tw.Flush();
            }

            return result;
        }

        private static void Dump(object obj, TextWriter tw, int currentDepth)
        {
            int textTabs = currentDepth + 1;

            NewLine(tw);
            WriteText("{", currentDepth, tw);
            NewLine(tw);

            if (obj == null || obj is ValueType || obj is string)
            {
                WriteValue(obj, tw);
            }
            else
            {
                MemberInfo[] members = obj.GetType().GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < members.Length; i++)
                {
                    MemberInfo member = members[i];

                    if (IgnoreElement(member))
                        continue;

                    ClassMember cm = new ClassMember(member);
                    if (cm.IsValid())
                    {
                        object value = cm.GetValue(obj);
                        if (cm.IsValueType() || cm.IsString())
                        {
                            WriteName(cm, textTabs, tw);
                            WriteValue(value, tw);
                        }
                        else if (cm.IsEnumerable())
                        {                            
                            WriteName(cm, textTabs, tw);
                            if (!InspectDeepness(tw, currentDepth))
                            {
                                IEnumerable enumerable = value as IEnumerable;
                                if (enumerable == null)
                                {
                                    WriteValue(enumerable, tw);
                                }
                                else
                                {
                                    foreach (object item in enumerable)
                                        Dump(item, tw, currentDepth + 1);
                                }
                            }
                        }
                        else
                        {
                                                       
                            WriteName(cm, textTabs, tw);
                            if (value == null)
                            {
                                WriteValue(value, tw);
                            }
                            else
                            {
                                if (!InspectDeepness(tw, currentDepth))
                                    Dump(value, tw, currentDepth + 1);
                            }
                        }

                        if (i + 1 < members.Length)
                            NewLine(tw);
                    }
                }

            }

            NewLine(tw);

            WriteText("}", currentDepth, tw);
        }

       

        private static bool IgnoreElement(MemberInfo member)
        {
            if (Settings.WriteCompilerGeneratedTypes)
                return false;                        
            
            var result = member.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true);
          
            return result.Length != 0;
        }
        

        private static bool InspectDeepness(TextWriter tw, int depth)
        {
            if (depth >= Settings.MaxDepth)
            {
                WriteText($"...............!!! TOO DEEP, DEPTH: {depth}/{Settings.MaxDepth} !!!..............", 1, tw);
                return true;
            }

            return false;

        }

        private static void NewLine(TextWriter tw)
        {
            tw.WriteLine();
        }

        private static void WriteName(ClassMember cm, int textTabs, TextWriter tw)
        {
            string name = cm.GetName().ToString();

            if (Settings.WriteElementType)
            {
                string type = cm.GetClassMemberType().FullName;
                WriteText($"{type} :: {name}= ", textTabs, tw);
            }                   
            else
                WriteText($"{name}= ", textTabs, tw);

        }

        private static void WriteText(string text, int tabs, TextWriter tw)
        {
            tw.Write(string.Empty.PadRight(tabs, '\t'));
            tw.Write(text);
        }

        private static void WriteValue(object value, TextWriter tw)
        {
            string v = value == null ? "null" : $"[{value.ToString()}]";

            WriteText(v, 1, tw);
        }
    }
}
