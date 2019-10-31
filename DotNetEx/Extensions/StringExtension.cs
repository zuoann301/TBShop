using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// StringExtensions
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 判断 s 是否为 null 或者空字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        /// <summary>
        /// 判断 s 是否为 null、空还是仅由空白字符。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
        /// <summary>
        /// 判断 s 是否不为 null 或者空字符串。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }
        /// <summary>
        /// 判断 s 是否不为 null、空还是仅由空白字符。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
        public static bool Contains(this string s, string keyword, bool ignoreCase)
        {
            if (keyword == null || s.IsNullOrEmpty())
                return false;

            if (ignoreCase)
            {
                s = s.ToLower();
                keyword = keyword.ToLower();
            }

            return s.Contains(keyword);
        }
        /// <summary>
        /// 将字符串中的 Encoding.Default 编码为一个字节序列。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string s)
        {
            return s.ToBytes(Encoding.UTF8);
        }
        /// <summary>
        /// 将字符串中的所有字符编码为一个字节序列。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string s, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(s);
            return bytes;
        }

        /// <summary>
        /// 使用 Encoding.UTF8 对 s 加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToMD5(this string s)
        {
            return ToMD5(s, Encoding.UTF8);
        }
        public static string ToMD5(this string s, Encoding encoding)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = encoding.GetBytes(s);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns>string.Format(format, args)</returns>
        public static string ToFormat(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static bool? ToBoolean(this string s, bool? defaultValue = null)
        {
            bool val;
            if (bool.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static byte? ToByte(this string s, byte? defaultValue = null)
        {
            byte val;
            if (byte.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static short? ToShort(this string s, short? defaultValue = null)
        {
            short val;
            if (short.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static int? ToInt(this string s, int? defaultValue = null)
        {
            int val;
            if (int.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static long? ToLong(this string s, long? defaultValue = null)
        {
            long val;
            if (long.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static float? ToFloat(this string s, float? defaultValue = null)
        {
            float val;
            if (float.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static double? ToDouble(this string s, double? defaultValue = null)
        {
            double val;
            if (double.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static decimal? ToDecimal(this string s, decimal? defaultValue = null)
        {
            decimal val;
            if (decimal.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static Guid? ToGuid(this string s, Guid? defaultValue = null)
        {
            Guid val;
            if (Guid.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static DateTime? ToDateTime(this string s, DateTime? defaultValue = null)
        {
            DateTime val;
            if (DateTime.TryParse(s, out val))
                return val;

            return defaultValue;
        }
        public static TEnum? ToEnum<TEnum>(this string s, TEnum? defaultValue = null) where TEnum : struct
        {
            int val;
            if (!int.TryParse(s, out val))
            {
                return defaultValue;
            }

            TEnum e = (TEnum)Enum.ToObject(typeof(TEnum), val);
            return e;
        }

        /// <summary>
        /// 使用 StringSplitOptions.RemoveEmptyEntries 分割字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> SplitString(this string s, params char[] separator)
        {
            return SplitStringImpl<string>(s, separator);
        }
        public static List<string> SplitString(this string s, char separator = ',')
        {
            return SplitStringImpl<string>(s, separator);
        }

        /// <summary>
        /// 使用 StringSplitOptions.RemoveEmptyEntries 分割字符串，并转成 T 类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<T> SplitString<T>(this string s, params char[] separator)
        {
            return SplitStringImpl<T>(s, separator);
        }

        static List<T> SplitStringImpl<T>(this string s, params char[] separator)
        {
            List<T> retList = new List<T>();

            if (string.IsNullOrEmpty(s))
                return retList;

            string[] arr = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in arr)
            {
                if (item is T)
                {
                    retList.Add((T)(object)item);
                }
                else
                    retList.Add((T)Convert.ChangeType(item, typeof(T)));
            }

            return retList;
        }
    }
}
