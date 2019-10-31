using Ace.Application;
using Ace.AutoMapper;
using Ace.Entity;
using Ace.Entity.System;
using Ace.IdStrategy;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.System
{
    public interface IOrgService : IAppService
    {
        List<SysOrg> GetList(string keyword = "");
        void Add(AddOrgInput input);
        void Update(UpdateOrgInput input);
        void Delete(string id, string operatorId);
        List<SysOrgPermission> GetPermissions(string orgId);
        void SetPermission(string orgId, List<string> permissionList);
        List<SysOrg> GetParentOrgs(int orgType);
        List<SysOrgType> GetOrgTypes();
        List<SysOrg> GetListById(List<string> ids);
    }

    public class OrgService : AppServiceBase<SysOrg>, IOrgService
    {
        public OrgService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<SysOrg> GetList(string keyword = "")
        {
            var q = this.Query.FilterDeleted();
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Name.Contains(keyword));
            }

            var ret = q.ToList();
            return ret;
        }
        public void Add(AddOrgInput input)
        {
            this.InsertFromDto(input);
        }
        public void Update(UpdateOrgInput input)
        {
            this.UpdateFromDto(input);
        }

        public void Delete(string id, string operatorId)
        {
            this.SoftDelete(id, operatorId);
        }

        public List<SysOrgPermission> GetPermissions(string orgId)
        {
            return this.DbContext.Query<SysOrgPermission>().Where(t => t.OrgId == orgId).ToList();
        }
        public void SetPermission(string orgId, List<string> permissionList)
        {
            orgId.NotNullOrEmpty();

            List<SysOrgPermission> roleAuths = permissionList.Select(a => new SysOrgPermission()
            {
                Id = IdHelper.CreateStringSnowflakeId(),
                OrgId = orgId,
                PermissionId = a
            }).ToList();

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Delete<SysOrgPermission>(a => a.OrgId == orgId);
                this.DbContext.InsertRange(roleAuths);
            });
        }
        public List<SysOrg> GetParentOrgs(int orgType)
        {
            var orgTypeQuery = this.DbContext.Query<SysOrgType>().Where(a => a.Id == orgType);
            var q = this.Query.Where(a => Sql.Equals(a.OrgType, orgTypeQuery.First().ParentId));
            List<SysOrg> ret = q.ToList();

            return ret;
        }
        public List<SysOrgType> GetOrgTypes()
        {
            return this.DbContext.Query<SysOrgType>().ToList();
        }
        public List<SysOrg> GetListById(List<string> ids)
        {
            if (ids.Count == 0)
                return new List<SysOrg>();

            return this.Query.FilterDeleted().Where(a => ids.Contains(a.Id)).ToList();
        }
    }

}
