using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    public enum PermissionType
    {
        /// <summary>
        /// 授权可见的菜单
        /// </summary>
        权限菜单 = 1,
        /// <summary>
        /// 登录即可见的菜单
        /// </summary>
        公共菜单 = 2,
        /// <summary>
        /// 受权限保护的操作
        /// </summary>
        权限项 = 3,
        /// <summary>
        /// 节点分组
        /// </summary>
        节点组 = 4,
    }

    [Table("Users_Permission")]
    public class Users_Permission
    {
        public string Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 权限码
        /// </summary>
        public string Code { get; set; }
        public string ParentId { get; set; }
        public PermissionType Type { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int? SortCode { get; set; }

        public List<Users_Permission> Children { get; set; } = new List<Users_Permission>();

        public int GetSortCode(int nullIf = 999)
        {
            if (this.SortCode == null)
                return nullIf;

            return this.SortCode.Value;
        }
    }
}
