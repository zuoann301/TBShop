using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Product_Size")]
    public class Product_Size
    {
        public string Id { get; set; }

        public string ProductCode { get; set; }

        public string ProSize { get; set; }

        public decimal Price { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 批发价
        /// </summary>
        public decimal BatchPrice { get; set; }

         

        public string CreateID { get; set; }

        public DateTime CreateDate { get; set; }


        public int ShopID { get; set; }


    }
}
