using Ace.Data;
using Ace.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.Implements
{
    public class SysLogAppService : ISysLogAppService
    {
        public void Log(LogType logType, string moduleName, bool? result, string description)
        {
        }
        public void Log(string userId, string realName, string ip, LogType logType, string moduleName, bool? result, string description)
        {
        }
        public Task LogAsync(LogType logType, string moduleName, bool? result, string description)
        {
            return Task.Run(() =>
            {

            });
        }
        public Task LogAsync(string userId, string realName, string ip, LogType logType, string moduleName, bool? result, string description)
        {
            return Task.Run(() =>
            {

            });
        }

        public void Dispose()
        {
        }
    }
}
