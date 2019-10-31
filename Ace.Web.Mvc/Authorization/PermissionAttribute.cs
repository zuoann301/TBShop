using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Web.Mvc.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PermissionAttribute : Attribute, IFilterMetadata
    {
        /*
         * 实现 IFilterMetadata，这样我们可以直接从 filterContext.ActionDescriptor.FilterDescriptors 中获取，避免使用反射获取 Attribute 的方式
         */
        public PermissionAttribute()
        {
        }
        public PermissionAttribute(string code)
        {
            this.Code = code;
        }

        public string Code { get; set; }
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class SkipPermissionAttribute : Attribute, IFilterMetadata
    {
    }
}
