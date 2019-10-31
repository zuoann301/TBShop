using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class ShopInputBase: ValidationModel
    {

         
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
        /// 
        /// </summary>
        public string ShopUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }


        public int ProvinceID { get; set; }

        public int CityID { get; set; }

        public int PartID { get; set; }


    }


    [MapToType(typeof(Shop))]
    public class AddShopInput : ShopInputBase
    { 
    }

    [MapToType(typeof(Shop))]
    public class UpdateShopInput : ShopInputBase
    {
        
        public int ID { get; set; }
    }
}
