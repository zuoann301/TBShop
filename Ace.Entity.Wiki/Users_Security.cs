using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("users_security")]
    public class Users_Security
    {
        public string Id { get; set; }

        public string UserID { get; set; }

        public decimal GPS_X { get; set; }

        public decimal GPS_Y { get; set; }

        public DateTime CreateDate { get; set; }
    }


    public class Users_SecurityInfo
    {
        public string Id { get; set; }

        public string UserID { get; set; }

        public decimal GPS_X { get; set; }

        public decimal GPS_Y { get; set; }

        public string UserName { get; set; }
        public string UserIcon { get; set; }

        public DateTime CreateDate { get; set; }
    }

}
