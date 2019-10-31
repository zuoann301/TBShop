using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("ShopOrderItem")]
    public class ShopOrderItem
    {
        public string Id { get; set; }

        public string OrderID { get; set; }

        public string ProductID { get; set; }
        public string ProSizeID { get; set; }
        public int ItemNum { get; set; }
        public decimal Price { get; set; }
        public string CreateID { get; set; }  


    }




    public class ShopOrderItemInfo
    {
        public string Id { get; set; }
        public string OrderID { get; set; }

        public string ProductID { get; set; }
        public string ProSizeID { get; set; }
        public int ItemNum { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string ProSize { get; set; }
        public string ImageUrl { get; set; } 


    }


}
