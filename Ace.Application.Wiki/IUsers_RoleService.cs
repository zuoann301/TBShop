using Ace.Application;
using Ace.AutoMapper;
using Ace.Entity;
using Ace.Entity.Wiki;
using Ace.IdStrategy;
using Chloe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application.Wiki
{
    public interface IUsers_RoleService : IAppService
    {
        List<Users_Role> GetRoles(string keyword = "");
        List<SimpleRoleModel> GetSimpleModels();
        void Add(AddUsers_RoleInput input);
        void Update(UpdateUsers_RoleInput input);
        void Delete(int ID);

        List<Users_Role> GetList(string keyword = "");
        List<Users_RolePermission> GetPermissions(int ID);
        void SetPermission(int ID, List<string> permissionList);
    }

    public class RoleService : AppServiceBase, IUsers_RoleService
    {
        public RoleService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Users_Role> GetRoles(string keyword = "")
        {
            var q = this.DbContext.Query<Users_Role>();
            if (keyword.IsNotNullOrEmpty())
            {
                q = q.Where(a => a.RoleName.Contains(keyword));
            }

            var ret = q.OrderBy(a => a.ID).ToList();
            return ret;
        }
        public List<SimpleRoleModel> GetSimpleModels()
        {
            var q = this.DbContext.Query<Users_Role>().FilterDeletedAndDisabled().OrderBy(a => a.ID);
            var ret = q.Select(a => new SimpleRoleModel() { ID = a.ID, RoleName = a.RoleName }).ToList();
            return ret;
        }
        public void Add(AddUsers_RoleInput input)
        {
            input.Validate();
            Users_Role role = this.CreateEntity<Users_Role>(null, null);

            AceMapper.Map(input, role);
            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Insert(role);
            });
        }
        public void Update(UpdateUsers_RoleInput input)
        {
            input.Validate();

            Users_Role role = this.DbContext.QueryByKey<Users_Role>(input.ID, true);

            role.NotNull("角色不存在");

            AceMapper.Map(input, role);

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Update(role);
            });
        }

        public void Delete(int ID)
        {
            this.DbContext.Delete<Users_Role>(a => a.ID == ID);
        }


        public List<Users_Role> GetList(string keyword = "")
        {
            var q = this.DbContext.Query<Users_Role>();
            if (!keyword.IsNullOrEmpty())
            {
                q = q.Where(a => a.RoleName.Contains(keyword));
            }
            
            var ret = q.OrderBy(a => a.ID).ToList();
            return ret;
        }
        public List<Users_RolePermission> GetPermissions(int ID)
        {
            return this.DbContext.Query<Users_RolePermission>().Where(t => t.RoleId == ID).ToList();
        }
        public void SetPermission(int ID, List<string> permissionList)
        {
            ID.NotNullOrEmpty();

            List<Users_RolePermission> roleAuths = permissionList.Select(a => new Users_RolePermission()
            {
                Id = IdHelper.CreateStringSnowflakeId(),
                RoleId = ID,
                PermissionId = a
            }).ToList();

            this.DbContext.DoWithTransaction(() =>
            {
                this.DbContext.Delete<Users_RolePermission>(a => a.RoleId == ID);
                this.DbContext.InsertRange(roleAuths);
            });
        }
    }

}
