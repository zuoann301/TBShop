using Ace.AutoMapper;
using Ace.Entity.Wiki;
using Ace.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Application.Wiki
{
    public class AddOrUpdateUsers_PermissionInputBase : ValidationModel
    {
        public string Name { get; set; }
        /// <summary>
        /// 权限码
        /// </summary>
        public string Code { get; set; }
        public string ParentId { get; set; }
        public PermissionType Type { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int? SortCode { get; set; }
    }

    [MapToType(typeof(Users_Permission))]
    public class AddUsers_PermissionInput : AddOrUpdateUsers_PermissionInputBase
    {
    }
    [MapToType(typeof(Users_Permission))]
    public class UpdateUsers_PermissionInput : AddOrUpdateUsers_PermissionInputBase
    {
        public string Id { get; set; }
        public override void Validate()
        {
            base.Validate();
            if (this.ParentId == this.Id)
                throw new InvalidInputException("上级节点不能为节点自身");
        }
    }
}
