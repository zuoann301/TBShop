using Ace.Application;
using Ace.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application
{
    public interface ISysLogAppService : IAppService
    {
        void Log(LogType logType, string moduleName, bool? result, string description);
        void Log(string userId, string realName, string ip, LogType logType, string moduleName, bool? result, string description);
        Task LogAsync(LogType logType, string moduleName, bool? result, string description);
        Task LogAsync(string userId, string realName, string ip, LogType logType, string moduleName, bool? result, string description);
    }
}
