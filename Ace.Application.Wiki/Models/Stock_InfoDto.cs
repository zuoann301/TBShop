using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class Stock_InfoInputBase : ValidationModel
    { 

        /// <summary>
        /// 入库单号/出库单号
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string ProSize { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }


    [MapToType(typeof(Stock_Info))]
    public class AddStock_InfoInput : Stock_InfoInputBase
    {
    }

    [MapToType(typeof(Pro_Sort))]
    public class UpdateStock_InfoInput : Stock_InfoInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
