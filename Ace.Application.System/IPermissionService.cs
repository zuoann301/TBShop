using Ace.Entity.System;
using Ace.Exceptions;
using Chloe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Application.System
{
    public interface IPermissionService : IAppService
    {
        List<SysPermission> GetList(PermissionType? type = null);
        List<SysPermission> GetPermissionMenus();
        void Add(AddPermissionInput input);
        void Update(UpdatePermissionInput input);
        void DeleteById(string id);
        void UpdateOrder(Dictionary<string, int> orderInfo);
    }

    public class PermissionService : AppServiceBase<SysPermission>, IPermissionService
    {
        public PermissionService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public List<SysPermission> GetList(PermissionType? type = null)
        {
            var q = this.DbContext.Query<SysPermission>();
            q = q.WhereIfNotNull(type, a => a.Type == type);
            return q.OrderBy(t => t.SortCode).ToList();
        }
        public List<SysPermission> GetPermissionMenus()
        {
            var q = this.DbContext.Query<SysPermission>();
            q = q.Where(a => a.Type == PermissionType.节点组 || a.Type == PermissionType.公共菜单 || a.Type == PermissionType.权限菜单);
            return q.ToList();
        }

        public void Add(AddPermissionInput input)
        {
            input.Validate();

            this.EnsurePermitUnique(input.Code, null);
            this.InsertFromDto<AddPermissionInput>(input);
        }
        public void Update(UpdatePermissionInput input)
        {
            input.Validate();

            this.EnsurePermitUnique(input.Code, input.Id);
            this.UpdateFromDto<UpdatePermissionInput>(input);
        }
        void EnsurePermitUnique(string permissionCode, string id)
        {
            if (permissionCode.IsNotNullOrEmpty())
            {
                bool exists = this.DbContext.Query<SysPermission>().WhereIfNotNull(id, a => a.Id != id).Any(a => a.Code == permissionCode);
                if (exists)
                    throw new InvalidInputException($"权限码 {permissionCode} 已存在，无法重复添加");
            }
        }

        public void DeleteById(string id)
        {
            id.NotNullOrEmpty();

            bool existsChildren = this.DbContext.Query<SysPermission>(a => a.ParentId == id).Any();
            if (existsChildren)
                throw new InvalidInputException("删除失败！操作的对象包含了下级数据");

            this.DbContext.DeleteByKey<SysPermission>(id);
        }

        public void UpdateOrder(Dictionary<string, int> orderInfo)
        {
            this.DbContext.DoWithTransaction(() =>
            {
                foreach (var kv in orderInfo)
                {
                    this.DbContext.Update<SysPermission>(a => a.Id == kv.Key, a => new SysPermission() { SortCode = kv.Value });
                }
            });
        }
    }

}
