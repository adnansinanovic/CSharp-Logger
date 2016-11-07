using System;
using System.Collections.Generic;
using System.Reflection;

namespace Logger
{
    public class ObjectDumperSettings
    {
        Dictionary<Type, IDumpFormatter> _formatters;
        public ObjectDumperSettings()
        {
            _formatters = new Dictionary<Type, IDumpFormatter>();
            WriteCompilerGeneratedTypes = false;
            MaxDepth = 4;
            WriteElementType = false;
            BindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        }

        /// <summary>
        /// Write compiler generated elements (e.g. backing fields)
        /// </summary>
        public bool WriteCompilerGeneratedTypes { get; set; }

        /// <summary>
        /// How deep dumper will go into object.
        /// </summary>
        public int MaxDepth { get; set; }

        public bool WriteElementType { get; set; }

        public BindingFlags BindingFlags { get; set; }

        public void AddFormatter(IDumpFormatter formatter)
        {
            _formatters.Add(formatter.FormatterType, formatter);
        }

        public IDumpFormatter GetFormatter(Type formatterType)
        {
            if (_formatters.ContainsKey(formatterType))
                return _formatters[formatterType];

            return null;
        }
        public bool RemoveFormatter(Type formatterType)
        {
            return _formatters.Remove(formatterType);
        }          
    }
}
