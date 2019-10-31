using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    public class Comment
    {
        public string Id { get; set; }

        public string Summary { get; set; }

        public string UserID { get; set; }

        public string Fid { get; set; }

        public DateTime CreateDate { get; set; } 

    }
}
