using Ace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public static class SessionExtension
    {
        public static void Set(this ISession session, string key, object obj)
        {
            if (obj == null)
                return;

            string str = obj as string;
            if (str == null)
                str = JsonHelper.Serialize(obj);

            session.SetString(key, str);
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default(T) : JsonHelper.Deserialize<T>(value);
        }
    }
}
