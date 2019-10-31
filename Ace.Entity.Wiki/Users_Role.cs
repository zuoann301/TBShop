using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Users_Role")]
    public class Users_Role
    {
        public int ID { get; set; }

        public string RoleName { get; set; }

        public int ST { get; set; }

    }
}
