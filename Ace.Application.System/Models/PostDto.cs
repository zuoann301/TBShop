using Ace.AutoMapper;
using Ace.Entity.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ace.Application.System
{
    public class PostInputBase : ValidationModel
    {
        public string Name { get; set; }
        public string OrgId { get; set; }
        public string Description { get; set; }
    }

    [MapToType(typeof(SysPost))]
    public class AddPostInput : PostInputBase
    {
    }

    [MapToType(typeof(SysPost))]
    public class UpdatePostInput : PostInputBase
    {
        [RequiredAttribute(ErrorMessage = "{0}不能为空")]
        public string Id { get; set; }
    }
}
