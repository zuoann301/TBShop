using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class Users_AddressInputBase: ValidationModel
    {
        

        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string CreateID { get; set; }

        public int IsDefault { get; set; }

        public decimal GPS_X { get; set; }

        public decimal GPS_Y { get; set; }

    }


    [MapToType(typeof(Users_Address))]
    public class AddUsers_AddressInput : Users_AddressInputBase
    { 
    }

    [MapToType(typeof(Users_Address))]
    public class UpdateUsers_AddressInput : Users_AddressInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
