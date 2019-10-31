using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Entity.System
{
    [Table("Sys_User")]
    public class SysUser
    {
        public const string _AdminAccountName = "admin";
        public static string AdminAccountName { get { return _AdminAccountName; } }

        public bool IsAdmin()
        {
            return this.AccountName != null && this.AccountName.ToLower() == _AdminAccountName;
        }
        public void EnsureIsNotAdmin()
        {
            if (this.IsAdmin())
                throw new Exception("");
        }

        public List<SysUserOrg> UserOrgs { get; set; } = new List<SysUserOrg>();
        public List<SysPost> Posts { get; set; } = new List<SysPost>();
        public List<SysRole> Roles { get; set; } = new List<SysRole>();

        public string Id { get; set; }
        public string AccountName { get; set; }
        public string Name { get; set; }
        public string HeadIcon { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WeChat { get; set; }

        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public string LastModifyUserId { get; set; }

        public AccountState State { get; set; } = AccountState.Normal;

        /// <summary>
        /// 以 , 分隔
        /// </summary>
        public string OrgIds { get; set; }
        public string PostIds { get; set; }
        public string RoleIds { get; set; }
    }
}
