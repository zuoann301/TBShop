using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.System
{
    [Table("Sys_UserPost")]
    public class SysUserPost
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
    }
}
