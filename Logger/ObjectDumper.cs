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
            // SPECIAL CASE - formatter is available
            if (obj != null)
            {
                IDumpFormatter formatter = Settings.GetFormatter(obj.GetType());
                if (formatter != null)
                {
                    tw.Write(formatter.Format(obj));
                    return;
                }
            }

            // SPECIAL CASE - primitive types: nulls, strings, numbers, characters...
            if (obj == null || obj is ValueType || obj is string)
            {
                WriteValue(obj, tw);
            }
            //SPECIAL CASE - Lists, arrays...
            else if (obj is IEnumerable)
            {
                IEnumerable collection = obj as IEnumerable;
                foreach (object item in collection)
                {
                    NewLine(tw);
                    Dump(item, tw, currentDepth);
                }
            }
            else
            {
                ObjectDump(obj, tw, currentDepth);
            }            
        }

        private static void ObjectDump(object obj, TextWriter tw, int currentDepth)
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
                MemberInfo[] members = obj.GetType().GetMembers(Settings.BindingFlags);
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
                            if (!InspectDeepness(cm, tw, currentDepth))
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
                                if (!InspectDeepness(cm, tw, currentDepth))
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

        private static bool InspectDeepness(ClassMember cm, TextWriter tw, int depth)
        {
            if (depth >= Settings.MaxDepth)
            {
                WriteText(string.Format("{0}{1}{2}", "{", cm.GetClassMemberType().ToString(), "}"), 0, tw);                
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
                WriteText($"{type}.{name} = ", textTabs, tw);
            }
            else
                WriteText($"{name} = ", textTabs, tw);
        }

        private static void WriteText(string text, int tabs, TextWriter tw)
        {
            tw.Write(string.Empty.PadRight(tabs, '\t'));
            tw.Write(text);
        }

        private static void WriteValue(object value, TextWriter tw)
        {
            bool isString = (value is string);

            string v = value == null ? "null" : isString ? $"\"{value.ToString()}\"" : value.ToString();

            WriteText(v, 0, tw);
        }
    }
}
