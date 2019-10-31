using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{
    [Table("Stock")]
    public class Stock
    {
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
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
}
