using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class CartInputBase: ValidationModel
    { 
        public string ProductID { get; set; }

        public string ProSizeID { get; set; }

        public int ItemNum { get; set; }

        public decimal Price { get; set; }

        public string CreateID { get; set; }

        public DateTime CreateDate { get; set; }

        public int ShopID { get; set; }

    }


    [MapToType(typeof(Cart))]
    public class AddCartInput : CartInputBase
    { 
    }

    [MapToType(typeof(Cart))]
    public class UpdateCartInput : CartInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
