using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class SortInputBase: ValidationModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Pid { get; set; }

         
    }


    [MapToType(typeof(Sort))]
    public class AddSortInput : SortInputBase
    {
    }

    [MapToType(typeof(Sort))]
    public class UpdateSortInput : SortInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public int Id { get; set; }
    }
}
