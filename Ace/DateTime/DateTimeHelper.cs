using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeHelper
    {
        public static DateTime DateOf1970_01_01 = new DateTime(1970, 1, 1);
        static TimeZoneInfo _ChinaStandardTimeTimeZone;
        public static TimeZoneInfo GetChinaStandardTimeTimeZone()
        {
            if (null == _ChinaStandardTimeTimeZone)
            {
                TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                _ChinaStandardTimeTimeZone = timezone;
            }

            return _ChinaStandardTimeTimeZone;
        }

        /// <summary>
        /// 获取北京标准时间
        /// </summary>
        public static DateTime GetChinaStandardTime()
        {
            TimeZoneInfo timezone = GetChinaStandardTimeTimeZone();
            DateTime datetime = TimeZoneInfo.ConvertTime(DateTime.Now, timezone);
            return datetime;
        }

        public static DateTime ToChinaStandardTime(this DateTime date)
        {
            TimeZoneInfo timezone = GetChinaStandardTimeTimeZone();
            DateTime datetime = TimeZoneInfo.ConvertTime(date, timezone);
            return datetime;
        }

        public static long TotalMilliseconds(this DateTime dateTime)
        {
            return (long)dateTime.Subtract(DateOf1970_01_01).TotalMilliseconds;
        }
        public static DateTime Parse(long totalMilliseconds)
        {
            return DateOf1970_01_01.AddMilliseconds(totalMilliseconds);
        }
    }
}
