using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.AutoMapper
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapToTypeAttribute : Attribute
    {
        bool _reverseMap = false;
        public MapToTypeAttribute(Type type) : this(type, false)
        {
        }
        public MapToTypeAttribute(Type type, bool reverseMap)
        {
            this.Type = type;
            this._reverseMap = reverseMap;
        }
        public Type Type { get; private set; }
        /// <summary>
        /// 是否反向映射，默认为 false
        /// </summary>
        public bool ReverseMap
        {
            get { return this._reverseMap; }
            set { this._reverseMap = value; }
        }
    }
}
