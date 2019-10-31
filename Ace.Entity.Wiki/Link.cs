using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Link")]
    public class Link
    {
        public string Id { get; set; }

        public int SortID { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string LinkUrl { get; set; }


        public int ShopID { get; set; }
    }
}
