using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;


namespace Ace.Entity.Wiki
{

    [Table("Product")]
    public class Product
    {

        public string ProSortID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductCode { get; set; }


        /// <summary>
        /// 成本价
        /// </summary>
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 批发价
        /// </summary>
        public decimal BatchPrice { get; set; }

        /// <summary>
        /// 分销比例
        /// </summary>
        public decimal SharePercent { get; set; }

        /// <summary>
        /// 零售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl { get; set; }

        public string ImageList { get; set; }


        


        /// <summary>
        /// 规格
        /// </summary>
        public string ProSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

        public int IsTop { get; set; }

        public int IsHot { get; set; }

        public string BrandID { get; set; }

        public string Contents { get; set; }

        public int Hit { get; set; }

        public int ShopID { get; set; }

    }


    public class ProductInfo
    {
        public Product product { get; set; }

        /// <summary>
        /// 是否收藏 1是  0否
        /// </summary>
        public int HasCollect { get; set; }

        /// <summary>
        /// 规格列表
        /// </summary>
        public List<Product_Size> SizeList { get; set; }

        public Brand brand { get; set; }

    }

    public class ProductSearchKey
    {
        public string ProductName { get; set; }
    }

}
