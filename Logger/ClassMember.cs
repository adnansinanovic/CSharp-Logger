using System;
using System.Collections;
using System.Reflection;

namespace Logger
{
    internal class ClassMember
    {
        FieldInfo _fieldInfo;
        PropertyInfo _propertyInfo;
        MemberInfo _memberInfo;

        public ClassMember(MemberInfo memberInfo)
        {
            _fieldInfo = memberInfo as FieldInfo;
            _propertyInfo = memberInfo as PropertyInfo;
            _memberInfo = memberInfo;
        }

        internal bool IsValid()
        {
            return _fieldInfo != null || _propertyInfo != null;
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
            if (_fieldInfo != null)
            {
                value = _fieldInfo.GetValue(obj);
            }
            else if (_propertyInfo != null)
            {
                var getMethod = _propertyInfo.GetGetMethod(false);
                if (getMethod != null)
                    value = _propertyInfo.GetValue(obj, null);
            }

            return value;
        }

        internal string GetName()
        {
            return _memberInfo.Name;
        }

        internal bool IsEnumerable()
        {
            Type t = GetClassMemberType();
            return typeof(IEnumerable).IsAssignableFrom(t);
        }

        public Type GetClassMemberType()
        {
            return _fieldInfo != null ? _fieldInfo.FieldType : _propertyInfo.PropertyType;
        }
    }
}
