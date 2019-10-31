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
    public interface IRoleService : IAppService
    {
        List<SysRole> GetRoles(string keyword = "");
        List<SimpleRoleModel> GetSimpleModels();
        void Add(AddRoleInput input);
        void Update(UpdateRoleInput input);
        void Delete(string roleId, string operatorId);

        List<SysRole> GetList(string keyword = "");
        List<SysRolePermission> GetPermissions(string id);
        void SetPermission(string id, List<string> permissionList);
    }

    public class RoleService : AppServiceBase, IRoleService
    {
        public RoleService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<SysRole> GetRoles(string keyword = "")
        {
            var q = this.DbContext.Query<SysRole>().FilterDeleted();
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.Name.Contains(keyword));
            }

            var ret = q.OrderBy(a => a.SortCode).ToList();
            return ret;
        }
        public List<SimpleRoleModel> GetSimpleModels()
        {
            var q = this.DbContext.Query<SysRole>().FilterDeletedAndDisabled().OrderBy(a => a.SortCode);
            var ret = q.Select(a => new SimpleRoleModel() { Id = a.Id, Name = a.Name }).ToList();
            return ret;
        }
        public void Add(AddRoleInput input)
        {
            input.Validate();
            SysRole role = this.CreateEntity<SysRole>(null, input.CreateUserId);

            AceMapper.Map(input, role);
            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Insert(role);
            });
        }
        public void Update(UpdateRoleInput input)
        {
            input.Validate();

            SysRole role = this.DbContext.QueryByKey<SysRole>(input.Id, true);

            role.NotNull("角色不存在");

            AceMapper.Map(input, role);

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Update(role);
            });
        }

        public void Delete(string id, string operatorId)
        {
            this.SoftDelete<SysRole>(id, operatorId);
        }


        public List<SysRole> GetList(string keyword = "")
        {
            var q = this.DbContext.Query<SysRole>().FilterDeleted();
            q = q.Where(a => a.Name.Contains(keyword));
            var ret = q.OrderBy(a => a.SortCode).ToList();
            return ret;
        }
        public List<SysRolePermission> GetPermissions(string id)
        {
            return this.DbContext.Query<SysRolePermission>().Where(t => t.RoleId == id).ToList();
        }
        public void SetPermission(string id, List<string> permissionList)
        {
            id.NotNullOrEmpty();

            List<SysRolePermission> roleAuths = permissionList.Select(a => new SysRolePermission()
            {
                Id = IdHelper.CreateStringSnowflakeId(),
                RoleId = id,
                PermissionId = a
            }).ToList();

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Delete<SysRolePermission>(a => a.RoleId == id);
                this.DbContext.InsertRange(roleAuths);
            });
        }
    }

}
