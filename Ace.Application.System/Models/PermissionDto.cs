using Ace.AutoMapper;
using Ace.Entity.System;
using Ace.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Application.System
{
    public class AddOrUpdatePermissionInputBase : ValidationModel
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

    [MapToType(typeof(SysPermission))]
    public class AddPermissionInput : AddOrUpdatePermissionInputBase
    {
    }
    [MapToType(typeof(SysPermission))]
    public class UpdatePermissionInput : AddOrUpdatePermissionInputBase
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
