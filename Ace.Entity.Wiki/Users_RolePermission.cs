using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Users_RolePermission")]
    public class Users_RolePermission
    {
        public string Id { get; set; }
        public int RoleId { get; set; }
        public string PermissionId { get; set; }
    }
}
