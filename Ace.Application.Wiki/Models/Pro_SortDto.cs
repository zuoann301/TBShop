using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class Pro_SortInputBase: ValidationModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        public string Summary { get; set; }

        public string Icon { get; set; }

        public string ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pid { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }

        public int ProCount { get; set; }


        public int ShopID { get; set; }
    }


    [MapToType(typeof(Pro_Sort))]
    public class AddPro_SortInput : Pro_SortInputBase
    {
    }

    [MapToType(typeof(Pro_Sort))]
    public class UpdatePro_SortInput : Pro_SortInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
