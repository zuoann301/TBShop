using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class Pro_CollectionInputBase: ValidationModel
    {
        public string ProductID { get; set; }

        public string UserID { get; set; }

        public DateTime CreateDate { get; set; }



    }


    [MapToType(typeof(Pro_Collection))]
    public class AddPro_CollectionInput : Pro_CollectionInputBase
    { 
    }

    [MapToType(typeof(Pro_Collection))]
    public class UpdatePro_CollectionInput : Pro_CollectionInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
