using System;
using System.Collections;
using System.Reflection;

namespace Logger
{
    internal class ClassMember
    {
        FieldInfo fieldInfo;
        PropertyInfo propertyInfo;
        MemberInfo memberInfo;

        public ClassMember(MemberInfo memberInfo)
        {
            fieldInfo = memberInfo as FieldInfo;
            propertyInfo = memberInfo as PropertyInfo;
            this.memberInfo = memberInfo;
        }

        internal bool IsValid()
        {
            return fieldInfo != null || propertyInfo != null;
        }

        internal bool IsValueType()
        {
            Type t = GetClassMemberType();
            return t.IsValueType;
        }

        internal bool IsString()
        {
            Type t = GetClassMemberType();
            return t == typeof(string);
        }

        internal object GetValue(object obj)
        {
            object value = null;
            if (fieldInfo != null)
            {
                value = fieldInfo.GetValue(obj);
            }
            else if (propertyInfo != null)
            {                
                if (propertyInfo.GetGetMethod(true) != null)
                    value = propertyInfo.GetValue(obj, null);
            }
            
            return value;
        }

        internal string GetName()
        {
            return memberInfo.Name;
        }

        internal bool IsEnumerable()
        {
            Type t = GetClassMemberType();
            return typeof(IEnumerable).IsAssignableFrom(t);
        }

        public Type GetClassMemberType()
        {
            return fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;
        }
    }
}
