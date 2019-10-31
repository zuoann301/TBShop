using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.System
{
    [Table("Sys_Org")]
    public class SysOrg
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? OrgType { get; set; }
        public string ManagerId { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string DeleteUserId { get; set; }
    }
}
