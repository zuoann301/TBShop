using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Chloe;

namespace Ace.Application.Implements
{
    public class EntityAppService : AppServiceBase, IEntityAppService
    {
        public EntityAppService(IDbContext dbContext, IServiceProvider services) : base(dbContext, services)
        {
        }

        public TEntity GetByKey<TEntity>(object key)
        {
            return this.DbContext.QueryByKey<TEntity>(key);
        }
        public List<TEntity> GetList<TEntity>()
        {
            return this.DbContext.Query<TEntity>().ToList();
        }
        public List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbContext.Query<TEntity>(predicate).ToList();
        }
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbContext.Delete(predicate);
        }
        public int Delete<TEntity>(TEntity entity)
        {
            return this.DbContext.Delete(entity);
        }
        public int DeleteByKey<TEntity>(object key)
        {
            key.NotNullOrEmpty("key 不能为空");
            return this.DbContext.DeleteByKey<TEntity>(key);
        }
        public int SoftDelete<TEntity>(object key)
        {
            key.NotNullOrEmpty("key 不能为空");
            return this.DbContext.SoftDelete<TEntity>(key);
        }
        public new int SoftDelete<TEntity>(object key, object deleteUserId)
        {
            key.NotNullOrEmpty("key 不能为空");
            return this.DbContext.SoftDelete<TEntity>(key, deleteUserId);
        }
        public TEntity Add<TEntity>(TEntity entity)
        {
            return this.DbContext.Insert(entity);
        }
        public TEntity AddFromDto<TEntity, TDto>(TDto dto)
        {
            CheckData(dto);
            return this.DbContext.InsertFromDto<TEntity, TDto>(dto);
        }
        public int Update<TEntity>(TEntity entity)
        {
            return this.DbContext.Update(entity);
        }
        public int UpdateFromDto<TEntity, TDto>(TDto dto)
        {
            CheckData(dto);
            return this.DbContext.UpdateFromDto<TEntity, TDto>(dto);
        }


        static void CheckData(object dto)
        {
            ValidationModel validationModel = dto as ValidationModel;
            if (validationModel != null)
            {
                validationModel.Validate();
            }
        }
    }
}
