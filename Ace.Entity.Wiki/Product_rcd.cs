using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("product_rcd")]
    public class Product_rcd
    {
        public string Id { get; set; }

        public string ProductID { get; set; }

        public string UserID { get; set; }

        public DateTime CreateDate { get; set; }

        public int Hit { get; set; }

        public DateTime UpdateTime { get; set; }

    }

    public class Product_rcdInfo: Product_rcd
    {
        public decimal Price { get; set; }


        public string ImageUrl { get; set; }
        public string ProductName { get; set; }

        public string ShopName { get; set; }


    }
}
