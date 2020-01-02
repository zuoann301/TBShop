using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class LinkInputBase: ValidationModel
    { 
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LinkUrl { get; set; }

        public int ShopID { get; set; }
        

    }


    [MapToType(typeof(Link))]
    public class AddLinkInput : LinkInputBase
    { 
    }

    [MapToType(typeof(Link))]
    public class UpdateLinkInput : LinkInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
