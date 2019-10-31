using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;


namespace Ace.Entity.Wiki
{
    [Table("Cart")]
    public class Cart
    {
        public string Id { get; set; }

        public string ProductID { get; set; }

        public string ProSizeID { get; set; }

        public int ItemNum { get; set; }

        public decimal Price { get; set; }

        public string CreateID { get; set; }

        public DateTime CreateDate { get; set; }

        public int ST { get; set; }

        public int ShopID { get; set; }
         
    }


    public class CartInfo
    {
        public string Id { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public string ProSize { get; set; }
        public int ItemNum { get; set; }
        public string CreateID { get; set; }

        public string ImageUrl { get; set; }

        public int ST { get; set; }
    }

}
