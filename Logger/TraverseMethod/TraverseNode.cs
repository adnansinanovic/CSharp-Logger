using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Logger
{
    internal class TraverseNode
    {
        public string Name { get; set; }
        public string ItemType { get; set; }
        public object Parent { get; set; }

        public object Item { get; set; }
        public int Level { get; set; }

        public TraverseNode(string name, string itemType, object item, object parent, int level)
        {
            Name = name;
            ItemType = itemType;
            Item = item;
            Parent = parent;
            Level = level;                
        }

        internal IEnumerable<TraverseNode> GetChildren(ObjectDumperSettings settings)
        {
            List<TraverseNode> children = new List<TraverseNode>();

            if (Item == null || Item is ValueType || Item is string)
                return children;

            if (Item is IEnumerable)
                return children;

            List<MemberInfo> members = Item.GetType().GetMembers(settings.BindingFlags).ToList();
            foreach (MemberInfo member in members)
            {
                Type type = member.DeclaringType;
                if (type.FullName.Equals($"System.{type.Name}", StringComparison.Ordinal))
                    continue;

                if (type.FullName.Equals($"System.Reflection.{type.Name}", StringComparison.Ordinal))
                    continue;

                if (IgnoreCompilerGeneratedMember(member, settings))
                    continue;

                ClassMember cm = new ClassMember(member);
                if (cm.IsValid())
                {
                    object value = cm.GetValue(Item);
                    children.Add(new TraverseNode(cm.GetName(), cm.GetClassMemberType().FullName, value, this, Level + 1));
                }
            }

            return children.OrderBy(x => x.Name);
        }

        private bool IgnoreCompilerGeneratedMember(MemberInfo member, ObjectDumperSettings settings)
        {
            if (settings.WriteCompilerGeneratedTypes)
                return false;

            var result = member.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true);

            return result.Length != 0;
        }



        internal NameContainer GetNames()
        {
            return new NameContainer(Name, $"{ItemType}.{Name}");
        }        
    }
}
