using Ace.AutoMapper;
using Ace.Entity.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.System
{
    public class OrgInputBase : ValidationModel
    {
        public string Name { get; set; }
        public int? OrgType { get; set; }
        public string ManagerId { get; set; }
        public string ParentId { get; set; }
        public string Description { get; set; }
    }

    [MapToType(typeof(SysOrg))]
    public class AddOrgInput : OrgInputBase
    {
    }

    [MapToType(typeof(SysOrg))]
    public class UpdateOrgInput : OrgInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
