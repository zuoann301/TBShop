using Ace.AutoMapper;
using Ace.Entity.Wiki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.Wiki
{
    public class SimpleRoleModel
    {
        public int ID { get; set; }
        public string RoleName { get; set; }

        public static SimpleRoleModel Create(Users_Role role)
        {
            SimpleRoleModel roleModel = new SimpleRoleModel();

            roleModel.ID = role.ID;
            roleModel.RoleName = role.RoleName;

            return roleModel;
        }
    }

    public class AddOrUpdateRoleInputBase : ValidationModel
    {
        [RequiredAttribute(ErrorMessage = "角色名称不能为空")]
        public string RoleName { get; set; } 
        public int ST { get; set; }
        
        public string PermissionIds { get; set; }

        public string[] GetPermissionIds()
        {
            if (this.PermissionIds == null)
                return new string[0];

            return this.PermissionIds.Split(',');
        }
    }

    [MapToType(typeof(Users_Role))]
    public class AddRoleInput : AddOrUpdateRoleInputBase
    { 
    }

    [MapToType(typeof(Users_Role))]
    public class UpdateRoleInput : AddOrUpdateRoleInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public int ID { get; set; }
    }
}
