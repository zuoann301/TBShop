using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class Users_SecurityInputBase: ValidationModel
    {
         

        public string UserID { get; set; }

        public decimal GPS_X { get; set; }

        public decimal GPS_Y { get; set; }

        public DateTime CreateDate { get; set; }



    }


    [MapToType(typeof(Users_Security))]
    public class AddUsers_SecurityInput : Users_SecurityInputBase
    { 
    }

    [MapToType(typeof(Users_Security))]
    public class UpdateUsers_SecurityInput : Users_SecurityInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
