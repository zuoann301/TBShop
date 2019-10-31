using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class DistrictInputBase : ValidationModel
    {
       

        public string Name { get; set; }

        public int Level { get; set; }

        public int Pid { get; set; }



    }


    [MapToType(typeof(District))]
    public class AddDistrictInput : LinkInputBase
    { 
    }

    [MapToType(typeof(District))]
    public class UpdateDistrictInput : LinkInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public int ID { get; set; }
    }
}
