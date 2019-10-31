using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Entity.System
{
    [Table("Sys_Role")]
    public class SysRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? SortCode { get; set; }

        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public DateTime? CreationTime { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? LastModifyTime { get; set; }
        public string LastModifyUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string DeleteUserId { get; set; }
    }
}
