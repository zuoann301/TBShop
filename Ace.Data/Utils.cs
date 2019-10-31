using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    static class Utils
    {
        public static void CheckNull(object obj, string paramName = null)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
