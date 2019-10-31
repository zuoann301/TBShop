using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class Product_rcdInputBase: ValidationModel
    {
        public string ProductID { get; set; }

        public string UserID { get; set; }

        public DateTime CreateDate { get; set; }

        public int Hit { get; set; }

        public DateTime UpdateTime { get; set; }



    }


    [MapToType(typeof(Product_rcd))]
    public class AddProduct_rcdInput : Product_rcdInputBase
    { 
    }

    [MapToType(typeof(Product_rcd))]
    public class UpdateProduct_rcdInput : Product_rcdInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
