using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("users_address")]
    public class Users_Address
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Mobile { get; set; }
       
        public string Address { get; set; }

        public string CreateID { get; set; }

        public int IsDefault { get; set; }

        public decimal GPS_X { get; set; }

        public decimal GPS_Y { get; set; }
    }
}
