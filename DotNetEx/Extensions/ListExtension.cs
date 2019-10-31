using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static bool In<T>(this T obj, List<T> list)
        {
            return list.Contains(obj);
        }
    }
}
