using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class MemberInfoExtension
    {
        /// <summary>
        /// 获取属性或字段的 Type 类型对象。
        /// </summary>
        /// <param name="propertyOrField"></param>
        /// <returns></returns>
        public static Type GetPropertyOrFieldType(this MemberInfo propertyOrField)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
                return ((PropertyInfo)propertyOrField).PropertyType;
            if (propertyOrField.MemberType == MemberTypes.Field)
                return ((FieldInfo)propertyOrField).FieldType;

            throw new NotSupportedException();
        }

        /// <summary>
        /// 设置属性或字段
        /// </summary>
        /// <param name="propertyOrField"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetPropertyOrFieldValue(this MemberInfo propertyOrField, object obj, object value)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
                ((PropertyInfo)propertyOrField).SetValue(obj, value, null);
            else if (propertyOrField.MemberType == MemberTypes.Field)
                ((FieldInfo)propertyOrField).SetValue(obj, value);

            throw new ArgumentException();
        }

        /// <summary>
        /// 获取属性或字段的值
        /// </summary>
        /// <param name="propertyOrField"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object GetPropertyOrFieldValue(this MemberInfo propertyOrField, object obj)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
                return ((PropertyInfo)propertyOrField).GetValue(obj, null);
            else if (propertyOrField.MemberType == MemberTypes.Field)
                return ((FieldInfo)propertyOrField).GetValue(obj);

            throw new ArgumentException();
        }

    }
}
