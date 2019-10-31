using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.System
{
    [Table("Sys_Post")]
    public class SysPost
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OrgId { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
