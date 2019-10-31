using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class NewsInputBase: ValidationModel
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

        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }

        public string Contents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }

        public int IsValid { get; set; }

    }


    [MapToType(typeof(News))]
    public class AddNewsInput : NewsInputBase
    { 
    }

    [MapToType(typeof(News))]
    public class UpdateNewsInput : NewsInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
