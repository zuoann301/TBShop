using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Shop")]
    public class Shop
    {

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        public int BrandID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShopAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ShopTel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GPS_X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GPS_Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 商家门面图
        /// </summary>
        public string ShopUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }



        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        public int PartID { get; set; }

 

        /// <summary>
        /// 商家状态  0未支付 1审核中 2正常
        /// </summary>
        public int ST { get; set; }

    }

    public class ShopSample
    {
        //ID, ShopName, ShopAddress, ShopTel, ShopUrl, Distance, GPS_X, GPS_Y
        public int ID { get; set; }

        public string ShopName { get; set; }
        public string ShopAddress { get; set; }

        public string ShopTel { get; set; }
        public string ShopUrl { get; set; }

        public int Distance { get; set; }

        public decimal GPS_X { get; set; }
        public decimal GPS_Y { get; set; }
    }

}
