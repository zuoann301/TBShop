using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity
{
    public enum AccountState
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 被禁用
        /// </summary>
        Disabled = 2,
        /// <summary>
        /// 已注销
        /// </summary>
        Closed = 3
    }
}
