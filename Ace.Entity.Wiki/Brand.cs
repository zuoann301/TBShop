using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Brand")]
    public class Brand
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Remark { get; set; }

        public int IsTop { get; set; }

        public int ShopID { get; set; }

    }
}
