using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.AutoMapper
{
    /// <summary>
    /// 配合 MapFromTypeAttribute 使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MapFromMemberAttribute : Attribute
    {
        public MapFromMemberAttribute(string memberName)
        {
            this.MemberName = memberName;
        }
        public string MemberName { get; set; }
    }
}
