using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.System
{
    [Table("Sys_OrgPermission")]
    public class SysOrgPermission
    {
        public string Id { get; set; }
        public string OrgId { get; set; }
        public string PermissionId { get; set; }
    }
}
