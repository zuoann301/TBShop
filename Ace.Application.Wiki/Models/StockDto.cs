using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class StockInputBase : ValidationModel
    {
        public string Type { get; set; }
        /// <summary>
        /// 入库单号 出库单号
        /// </summary>
        public string StockCode { get; set; }

        /// <summary>
        /// 交货单位
        /// </summary>
        public string FromUserID { get; set; }

        /// <summary>
        /// 出库收货人/入库保管员
        /// </summary>
        public string ToUserID { get; set; }

        /// <summary>
        /// 审核人（会计）
        /// </summary>
        public string AuthUserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

        public DateTime ActionDate { get; set; }
    }


    [MapToType(typeof(Stock))]
    public class AddStockInput : StockInputBase
    {
    }

    [MapToType(typeof(Pro_Sort))]
    public class UpdateStockInput : StockInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
