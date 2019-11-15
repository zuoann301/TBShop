using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class ProductInputBase : ValidationModel
    {

        public string ProSortID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [RequiredAttribute(ErrorMessage = "产品名称不能为空")]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        [RequiredAttribute(ErrorMessage = "产品编码不能为空")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [RequiredAttribute(ErrorMessage = "单价不能为空")]
        public decimal Price { get; set; }


        /// <summary>
        /// 成本价
        /// </summary>
        [RequiredAttribute(ErrorMessage = "成本价不能为空")]
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 批发价
        /// </summary>
        [RequiredAttribute(ErrorMessage = "批发价不能为空")]
        public decimal BatchPrice { get; set; }

        /// <summary>
        /// 分销比例
        /// </summary>
        [RequiredAttribute(ErrorMessage = "分销比例不能为空")]
        public decimal SharePercent { get; set; }


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

        public string Contents { get; set; }

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

        public int Hit { get; set; }
    }


    [MapToType(typeof(Product))]
    public class AddProductInput : ProductInputBase
    {
    }

    [MapToType(typeof(Product))]
    public class UpdateProductInput : ProductInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
