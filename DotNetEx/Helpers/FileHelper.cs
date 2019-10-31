using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Utilities.Helpers
{
    public static class FileHelper
    {
        public static void DeleteIfExists(string fileFullName)
        {
            if (File.Exists(fileFullName))
            {
                File.Delete(fileFullName);
            }
        }
    }
}
