using Ace.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Application
{
    /// <summary>
    /// 对单个实体的简单操作。Tips：如涉及多表操作，请另写额外应用服务事务控制。
    /// </summary>
    public interface IEntityAppService : IAppService
    {
        TEntity GetByKey<TEntity>(object key);
        List<TEntity> GetList<TEntity>();
        List<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate);
        int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate);
        int Delete<TEntity>(TEntity entity);
        int DeleteByKey<TEntity>(object key);
        int SoftDelete<TEntity>(object key);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <param name="deleteUserId">执行删除操作的用户Id</param>
        /// <returns></returns>
        int SoftDelete<TEntity>(object key, object deleteUserId);
        TEntity Add<TEntity>(TEntity entity);
        /// <summary>
        /// 传入一个 dto 对象，插入相应的数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        TEntity AddFromDto<TEntity, TDto>(TDto dto);
        int Update<TEntity>(TEntity entity);
        /// <summary>
        /// 传入一个 dto 对象，更新相应的数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dto"></param>
        /// <returns></returns>
        int UpdateFromDto<TEntity, TDto>(TDto dto);
    }
}
