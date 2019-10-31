using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application
{
    public static class CurrentAssembly
    {
        public static Assembly Value
        {
            get
            {
                Assembly assembly = typeof(CurrentAssembly).GetTypeInfo().Assembly;
                return assembly;
            }
        }
    }
}
