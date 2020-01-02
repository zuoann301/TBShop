using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class BrandInputBase: ValidationModel
    {
        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Remark { get; set; }

        public int IsTop { get; set; }


        public int ShopID { get; set; }
    }


    [MapToType(typeof(Brand))]
    public class AddBrandInput : BrandInputBase
    { 
    }

    [MapToType(typeof(Brand))]
    public class UpdateBrandInput : BrandInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
