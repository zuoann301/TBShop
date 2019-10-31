using DotNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    public static class ReflectionExtension
    {
        public static Type GetMemberType(this MemberInfo propertyOrField)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
                return ((PropertyInfo)propertyOrField).PropertyType;
            if (propertyOrField.MemberType == MemberTypes.Field)
                return ((FieldInfo)propertyOrField).FieldType;

            throw new ArgumentException();
        }

        public static object GetMemberValue(this MemberInfo propertyOrField, object obj)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
                return ((PropertyInfo)propertyOrField).GetValue(obj, null);
            else if (propertyOrField.MemberType == MemberTypes.Field)
                return ((FieldInfo)propertyOrField).GetValue(obj);

            throw new ArgumentException();
        }
        public static void SetMemberValue(this MemberInfo propertyOrField, object obj, object value)
        {
            if (propertyOrField.MemberType == MemberTypes.Property)
                ((PropertyInfo)propertyOrField).SetValue(obj, value, null);
            else if (propertyOrField.MemberType == MemberTypes.Field)
                ((FieldInfo)propertyOrField).SetValue(obj, value);
            else
                throw new ArgumentException();
        }


        /// <summary>
        /// 获取 public 属性值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object instance, string propertyName)
        {
            dnUtils.NotNull(instance);
            dnUtils.NotNullOrEmpty(propertyName);

            Type instanceType = instance.GetType();

            var property = instanceType.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException(string.Format("The type of '{0}' do not define property '{0}'", instanceType.FullName, propertyName));

            object value = property.GetValue(instance);
            return value;
        }
        /// <summary>
        /// 获取 public 字段值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetFieldValue(this object instance, string fieldName)
        {
            dnUtils.NotNull(instance);
            dnUtils.NotNullOrEmpty(fieldName);

            Type instanceType = instance.GetType();

            var field = instanceType.GetField(fieldName);
            if (field == null)
                throw new ArgumentException(string.Format("The type of '{0}' do not define field '{0}'", instanceType.FullName, fieldName));

            object value = field.GetValue(instance);
            return value;
        }

        /// <summary>
        /// 获取 public 属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object instance, string propertyName)
        {
            object value = instance.GetPropertyValue(propertyName);
            return (T)value;
        }
        /// <summary>
        /// 获取 public 字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static T GetFieldValue<T>(this object instance, string fieldName)
        {
            object value = instance.GetFieldValue(fieldName);
            return (T)value;
        }

        /// <summary>
        /// 设置公共属性值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this object instance, string propertyName, object value)
        {
            dnUtils.NotNull(instance);
            dnUtils.NotNullOrEmpty(propertyName);

            Type instanceType = instance.GetType();

            var property = instanceType.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException(string.Format("The type of '{0}' do not define property '{0}'", instanceType.FullName, propertyName));

            property.SetValue(instance, value);
        }
        /// <summary>
        /// 设置公共字段值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue(this object instance, string fieldName, object value)
        {
            dnUtils.NotNull(instance);
            dnUtils.NotNullOrEmpty(fieldName);

            Type instanceType = instance.GetType();

            var field = instanceType.GetField(fieldName);
            if (field == null)
                throw new ArgumentException(string.Format("The type of '{0}' do not define field '{0}'", instanceType.FullName, fieldName));

            field.SetValue(instance, value);
        }

    }
}
