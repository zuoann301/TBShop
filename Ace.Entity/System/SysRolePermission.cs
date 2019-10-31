using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.System
{
    [Table("Sys_RolePermission")]
    public class SysRolePermission
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
    }
}
