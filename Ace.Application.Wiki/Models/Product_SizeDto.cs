using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class Product_SizeInputBase: ValidationModel
    {
        public string ProductCode { get; set; }

        public string ProSize { get; set; }


        [Display(Name = "价格")]
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        [RegularExpression("^[0-9]+(.[0-9]{2})?$", ErrorMessage = "{0}不能为空，且只能输入数字！")]
        public decimal Price { get; set; }


        /// <summary>
        /// 成本价
        /// </summary>
        [Display(Name = "成本价")]
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        [RegularExpression("^[0-9]+(.[0-9]{2})?$", ErrorMessage = "{0}不能为空，且只能输入数字！")]
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 批发价
        /// </summary>
        [Display(Name = "批发价")]
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        [RegularExpression("^[0-9]+(.[0-9]{2})?$", ErrorMessage = "{0}不能为空，且只能输入数字！")]
        public decimal BatchPrice { get; set; }

          

        public string CreateID { get; set; }

        public DateTime CreateDate { get; set; }
    }


    [MapToType(typeof(Product_Size))]
    public class AddProduct_SizeInput : Product_SizeInputBase
    {
    }

    [MapToType(typeof(Product_Size))]
    public class UpdateProduct_SizeInput : Product_SizeInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
