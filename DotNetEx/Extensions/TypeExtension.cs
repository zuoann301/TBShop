using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtension
    {
        static Dictionary<Type, object> _TypeDefaultValues = new Dictionary<Type, object>();

        static TypeExtension()
        {
            _TypeDefaultValues.Add(typeof(byte), default(byte));
            _TypeDefaultValues.Add(typeof(sbyte), default(sbyte));
            _TypeDefaultValues.Add(typeof(short), default(short));
            _TypeDefaultValues.Add(typeof(ushort), default(ushort));
            _TypeDefaultValues.Add(typeof(int), default(int));
            _TypeDefaultValues.Add(typeof(uint), default(uint));
            _TypeDefaultValues.Add(typeof(long), default(long));
            _TypeDefaultValues.Add(typeof(ulong), default(ulong));
            _TypeDefaultValues.Add(typeof(float), default(float));
            _TypeDefaultValues.Add(typeof(double), default(double));
            _TypeDefaultValues.Add(typeof(decimal), default(decimal));
            _TypeDefaultValues.Add(typeof(bool), default(bool));
            _TypeDefaultValues.Add(typeof(Guid), default(Guid));
            _TypeDefaultValues.Add(typeof(DateTime), default(DateTime));
            _TypeDefaultValues.Add(typeof(DateTimeOffset), default(DateTimeOffset));
            _TypeDefaultValues.Add(typeof(TimeSpan), default(TimeSpan));
        }

        public static bool IsNullable(this Type type)
        {
            Type underlyingType;
            return IsNullable(type, out underlyingType);
        }
        public static bool IsNullable(this Type type, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType != null;
        }
        /// <summary>
        /// 获取 type 的 UnderlyingType，如果 type 不是 Nullable 类型，则返回 type 本身
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(this Type type)
        {
            Type underlyingType;
            if (!IsNullable(type, out underlyingType))
                underlyingType = type;

            return underlyingType;
        }

        public static bool IsAnonymousType(this Type type)
        {
            string typeName = type.Name;
            return typeName.Contains("<>") && typeName.Contains("__") && typeName.Contains("AnonymousType");
        }

        public static object GetDefaultValue(this Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            if (typeInfo.IsClass || typeInfo.IsInterface || type.IsNullable())
            {
                return null;
            }
            else
            {
                /* 值类型 */

                object defaultValue;

                if (_TypeDefaultValues.TryGetValue(type, out defaultValue))
                {
                    return defaultValue;
                }

                var method = typeof(TypeExtension).GetMethod("GetDefaultValueOfType", BindingFlags.Static | BindingFlags.NonPublic);
                method = method.MakeGenericMethod(type);
                defaultValue = method.Invoke(null, null);
                return defaultValue;
            }
        }
        public static bool IsDefaultValueOfType(this object value, Type type)
        {
            object defaultValue = type.GetDefaultValue();
            return Utils.AreEqual(value, defaultValue);
        }

        static object GetDefaultValueOfType<T>()
        {
            return default(T);
        }
    }
}
