using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class ShopOrderInputBase : ValidationModel
    {
         
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

    }


    [MapToType(typeof(ShopOrder))]
    public class AddShopOrderInput : ShopOrderInputBase
    { 
    }

    [MapToType(typeof(ShopOrder))]
    public class UpdateShopOrderInput : ShopOrderInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
