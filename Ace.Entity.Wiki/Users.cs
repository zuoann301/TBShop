using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{

    [Table("Users")]
    public class Users
    {
        public string Id { get; set; }

        public string Mobile { get; set; }

        public string UserSecretkey { get; set; }


        public string UserName { get; set; }

        public string Email { get; set; }

        public int ST { get; set; }

        public int Sex { get; set; }

        public int RoleID { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime LastLoginDate { get; set; }


        public int ShopID { get; set; }


        /// <summary>
        /// 隶属来源ID
        /// </summary>
        public string FromID { get; set; }


        public string UserPass { get; set; }

        public string OpenID { get; set; }
        public string UserIcon { get; set; }


    }


    public class UsersInfo2
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }

}
