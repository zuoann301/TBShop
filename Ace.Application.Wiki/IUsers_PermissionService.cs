using Ace.Entity.Wiki;
using Ace.Exceptions;
using Chloe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Application.Wiki
{
    public interface IUsers_PermissionService : IAppService
    {
        List<Users_Permission> GetList(PermissionType? type = null);
        List<Users_Permission> GetPermissionMenus();
        void Add(AddUsers_PermissionInput input);
        void Update(UpdateUsers_PermissionInput input);
        void DeleteById(string id);
        void UpdateOrder(Dictionary<string, int> orderInfo);
    }

    public class Users_PermissionService : AppServiceBase<Users_Permission>, IUsers_PermissionService
    {
        public Users_PermissionService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<Users_Permission> GetList(PermissionType? type = null)
        {
            var q = this.DbContext.Query<Users_Permission>();
            q = q.WhereIfNotNull(type, a => a.Type == type);
            return q.OrderBy(t => t.SortCode).ToList();
        }
        public List<Users_Permission> GetPermissionMenus()
        {
            var q = this.DbContext.Query<Users_Permission>();
            q = q.Where(a => a.Type == PermissionType.节点组 || a.Type == PermissionType.公共菜单 || a.Type == PermissionType.权限菜单);
            return q.ToList();
        }

        public void Add(AddUsers_PermissionInput input)
        {
            input.Validate();

            this.EnsurePermitUnique(input.Code, null);
            this.InsertFromDto<AddUsers_PermissionInput>(input);
        }
        public void Update(UpdateUsers_PermissionInput input)
        {
            input.Validate();

            this.EnsurePermitUnique(input.Code, input.Id);
            this.UpdateFromDto<UpdateUsers_PermissionInput>(input);
        }
        void EnsurePermitUnique(string permissionCode, string id)
        {
            if (permissionCode.IsNotNullOrEmpty())
            {
                bool exists = this.DbContext.Query<Users_Permission>().WhereIfNotNull(id, a => a.Id != id).Any(a => a.Code == permissionCode);
                if (exists)
                    throw new InvalidInputException($"权限码 {permissionCode} 已存在，无法重复添加");
            }
        }

        public void DeleteById(string id)
        {
            id.NotNullOrEmpty();

            bool existsChildren = this.DbContext.Query<Users_Permission>(a => a.ParentId == id).Any();
            if (existsChildren)
                throw new InvalidInputException("删除失败！操作的对象包含了下级数据");

            this.DbContext.DeleteByKey<Users_Permission>(id);
        }

        public void UpdateOrder(Dictionary<string, int> orderInfo)
        {
            this.DbContext.DoWithTransaction(() =>
            {
                foreach (var kv in orderInfo)
                {
                    this.DbContext.Update<Users_Permission>(a => a.Id == kv.Key, a => new Users_Permission() { SortCode = kv.Value });
                }
            });
        }
    }

}
