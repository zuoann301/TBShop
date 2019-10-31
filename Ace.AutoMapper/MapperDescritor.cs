using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.AutoMapper
{
    public class MapperDescritor
    {
        public MapperDescritor(Type sourceType, Type targetType, bool reverseMap)
        {
            this.SourceType = sourceType;
            this.TargetType = targetType;
            this.ReverseMap = reverseMap;
            this.MemberRelationships = new List<MapperMemberRelationship>();
        }
        public Type SourceType { get; private set; }
        public Type TargetType { get; private set; }
        public bool ReverseMap { get; set; }

        public List<MapperMemberRelationship> MemberRelationships { get; private set; }
    }
}
