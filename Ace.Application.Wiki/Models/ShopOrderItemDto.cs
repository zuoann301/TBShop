using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class ShopOrderItemInputBase: ValidationModel
    { 

        public string OrderID { get; set; }

        public string ProductID { get; set; }
        public string ProSizeID { get; set; }
        public int ItemNum { get; set; }
        public decimal Price { get; set; }
        public string CreateID { get; set; }  


    }


    [MapToType(typeof(ShopOrderItem))]
    public class AddShopOrderItemInput : ShopOrderItemInputBase
    { 
    }

    [MapToType(typeof(ShopOrderItem))]
    public class UpdateShopOrderItemInput : ShopOrderItemInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
