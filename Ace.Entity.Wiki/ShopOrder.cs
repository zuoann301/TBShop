using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Entity.Wiki
{

    [Table("ShopOrder")]
    public class ShopOrder
    {
        public string Id { get; set; }

        public string OrderCode { get; set; }

        public decimal Total { get; set; }

        public decimal ProTotal { get; set; }
        public decimal Freight { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int ST { get; set; }
        /// <summary>
        /// 审核
        /// </summary>
        public string AuthID { get; set; }
        public string CreateID { get; set; } 

        public DateTime CreateDate { get; set; }

        public DateTime UpdateTime { get; set; }

        public string AddressID { get; set; }
        public int ShopID { get; set; }
    }

     
    public class ShopOrderInfo
    {
        public string Id { get; set; }

        public string OrderCode { get; set; }

        public decimal Total { get; set; }

        public string CreateName { get; set; }

        //public string ShopName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 审核
        /// </summary>
        public string AuthName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int ST { get; set; }

        public string ST_Name { get; set; }

        //public int ShopID { get; set; }
        public string CreateID { get; set; }
    }

     
    public class ShopOrderInfo2
    {
        public string Id { get; set; }

        public string OrderCode { get; set; }

        public decimal Total { get; set; }

        public decimal ProTotal { get; set; }
        public decimal Freight { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int ST { get; set; }

        public string STName { get; set; }
        /// <summary>
        /// 审核
        /// </summary>
        public string AuthID { get; set; }
        public string CreateID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateTime { get; set; }

        public string AddressID { get; set; }

        public List<ShopOrderItemInfo> OrderItem { get; set; }

        public Users_Address AddressInfo { get; set; }
    }

    public enum ShopOrderST
    {
        /// <summary>
        /// 待审核
        /// </summary>
        WaitForAuth = 0,

        /// <summary>
        /// 已审核
        /// </summary>
        Authed=1

        
    }

}
