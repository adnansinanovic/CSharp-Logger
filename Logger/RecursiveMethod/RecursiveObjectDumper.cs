using Logger.Fomatters;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Logger
{
    [Obsolete]
    public class RecursiveObjectDumper : IObjectDumper
    {
        public ObjectDumperSettings Settings { get; set; }
        private InternalObjectFormatter _formatter;

        public RecursiveObjectDumper()
        {
            _formatter = new InternalObjectFormatter();
            Settings = new ObjectDumperSettings();
        }

        public void Dump(object obj, TextWriter tw)
        {            
            Dump(obj, tw, 0);                                      
        }

        private void Dump(object obj, TextWriter tw, int currentDepth)
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
                _formatter.WriteValue(obj, tw);
            }
            //SPECIAL CASE - Lists, arrays...
            else if (obj is IEnumerable)
            {
                IEnumerable collection = obj as IEnumerable;
                foreach (object item in collection)
                {
                    _formatter.NewLine(tw);
                    Dump(item, tw, currentDepth);
                }
            }
            else
            {
                ObjectDump(obj, tw, currentDepth);
            }
        }

        private void ObjectDump(object obj, TextWriter tw, int currentDepth)
        {
            Type type = obj.GetType();
            if (type.FullName.Equals($"System.{type.Name}", StringComparison.Ordinal))
                return;

            if (type.FullName.Equals($"System.Reflection.{type.Name}", StringComparison.Ordinal))
                return;           

            int textTabs = currentDepth + 1;
            _formatter.NewLine(tw);
            _formatter.WriteText("{", currentDepth, tw);
            _formatter.NewLine(tw);

            if (obj == null || obj is ValueType || obj is string)
            {
                _formatter.WriteValue(obj, tw);
            }
            else
            {               
                MemberInfo[] members = type.GetMembers(Settings.BindingFlags);
                for (int i = 0; i < members.Length; i++)
                {
                    MemberInfo member = members[i];

                    if (IgnoreCompilerGeneratedMember(member))
                        continue;

                    ClassMember cm = new ClassMember(member);
                    if (cm.IsValid())
                    {
                        object value = cm.GetValue(obj);
                        if (cm.IsValueType() || cm.IsString())
                        {
                            _formatter.WriteName(cm.GetNames(), textTabs, tw, Settings.WriteElementType);
                            _formatter.WriteValue(value, tw);
                        }
                        else if (cm.IsEnumerable())
                        {
                            _formatter.WriteName(cm.GetNames(), textTabs, tw, Settings.WriteElementType);
                            if (!InspectDeepness(cm, tw, currentDepth))
                            {
                                IEnumerable enumerable = value as IEnumerable;
                                if (enumerable == null)
                                {
                                    _formatter.WriteValue(enumerable, tw);
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
                            _formatter.WriteName(cm.GetNames(), textTabs, tw, Settings.WriteElementType);
                            if (value == null)
                            {
                                _formatter.WriteValue(value, tw);
                            }
                            else
                            {
                                if (!InspectDeepness(cm, tw, currentDepth))
                                    Dump(value, tw, currentDepth + 1);
                            }
                        }

                        if (i + 1 < members.Length)
                            _formatter.NewLine(tw);
                    }
                }
            }

            _formatter.NewLine(tw);

            _formatter.WriteText("}", currentDepth, tw);
        }


        private bool IgnoreCompilerGeneratedMember(MemberInfo member)
        {            
            if (Settings.WriteCompilerGeneratedTypes)
                return false;

            var result = member.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true);

            return result.Length != 0;
        }

        private bool InspectDeepness(ClassMember cm, TextWriter tw, int depth)
        {
            if (depth >= Settings.MaxDepth)
            {
                _formatter.WriteText(string.Format("{0}{1}{2}", "{", cm.GetClassMemberType().ToString(), "}"), 0, tw);
                return true;
            }

            return false;
        }      
    }
}
