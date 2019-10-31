using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public enum ResultStatus
    {
        OK = 100,
        Failed = 101,
        /// <summary>
        /// 表示未登录
        /// </summary>
        NotLogin = 102,
        /// <summary>
        /// 表示未授权
        /// </summary>
        Unauthorized = 103,
    }
}
