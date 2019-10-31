using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.System
{
    [Table("Sys_UserOrg")]
    public class SysUserOrg
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string OrgId { get; set; }
        public bool DisablePermission { get; set; }
        public SysOrg Org { get; set; }
    }
}
