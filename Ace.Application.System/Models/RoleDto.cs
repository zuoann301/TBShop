using Ace.AutoMapper;
using Ace.Entity.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.System
{
    public class SimpleRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static SimpleRoleModel Create(SysRole role)
        {
            SimpleRoleModel roleModel = new SimpleRoleModel();

            roleModel.Id = role.Id;
            roleModel.Name = role.Name;

            return roleModel;
        }
    }

    public class AddOrUpdateRoleInputBase : ValidationModel
    {
        [RequiredAttribute(ErrorMessage = "角色名称不能为空")]
        public string Name { get; set; }
        public int? SortCode { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        public string PermissionIds { get; set; }

        public string[] GetPermissionIds()
        {
            if (this.PermissionIds == null)
                return new string[0];

            return this.PermissionIds.Split(',');
        }
    }

    [MapToType(typeof(SysRole))]
    public class AddRoleInput : AddOrUpdateRoleInputBase
    {
        public string CreateUserId { get; set; }
    }

    [MapToType(typeof(SysRole))]
    public class UpdateRoleInput : AddOrUpdateRoleInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
