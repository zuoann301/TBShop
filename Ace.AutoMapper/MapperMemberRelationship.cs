using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ace.AutoMapper
{
    public class MapperMemberRelationship
    {
        public MapperMemberRelationship(MemberInfo sourceMember, MemberInfo targetMember)
        {
            this.SourceMember = sourceMember;
            this.TargetMember = targetMember;
        }
        public MemberInfo SourceMember { get; private set; }
        public MemberInfo TargetMember { get; private set; }
    }
}
