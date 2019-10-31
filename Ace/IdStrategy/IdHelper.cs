using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.IdStrategy
{
    public static class IdHelper
    {
        public static string CreateGuid()
        {
            Guid id = Guid.NewGuid();
            return id.ToString("N").ToLower();
        }

        public static long CreateSnowflakeId()
        {
            return Snowflake.IdWorker.Instance.NextId();
        }
        public static string CreateStringSnowflakeId()
        {
            return CreateSnowflakeId().ToString();
        }
    }
}
