using Ace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chloe
{
    public static class ChloeQueryExtension
    {
        /// <summary>
        /// q.Where(a => a.IsDeleted == false)
        /// </summary>
        /// <typeparam name="TEntity">必须含有 IsDeleted 属性</typeparam>
        /// <param name="q"></param>
        /// <returns></returns>
        public static IQuery<TEntity> FilterDeleted<TEntity>(this IQuery<TEntity> q)
        {
            Type entityType = typeof(TEntity);

            PropertyInfo prop_IsDeleted = entityType.GetProperty("IsDeleted");

            if (prop_IsDeleted == null)
                throw new ArgumentException("实体类型未定义 IsDeleted 属性");

            ParameterExpression parameter = Expression.Parameter(entityType, "a");
            Expression prop = Expression.Property(parameter, prop_IsDeleted);

            Expression falseValue = Expression.Constant(false, prop_IsDeleted.PropertyType);

            Expression lambdaBody = Expression.Equal(prop, falseValue);

            Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(lambdaBody, parameter);

            return q.Where(predicate);
        }
        /// <summary>
        /// q.Where(a => a.IsEnabled == true)
        /// </summary>
        /// <typeparam name="TEntity">必须含有 IsEnabled 属性</typeparam>
        /// <param name="q"></param>
        /// <returns></returns>
        public static IQuery<TEntity> FilterDisabled<TEntity>(this IQuery<TEntity> q)
        {
            Type entityType = typeof(TEntity);

            PropertyInfo prop_IsEnabled = entityType.GetProperty("IsEnabled");

            if (prop_IsEnabled == null)
                throw new ArgumentException("实体类型未定义 IsEnabled 属性");

            ParameterExpression parameter = Expression.Parameter(entityType, "a");
            Expression prop = Expression.Property(parameter, prop_IsEnabled);

            Expression trueValue = Expression.Constant(true, prop_IsEnabled.PropertyType);

            Expression lambdaBody = Expression.Equal(prop, trueValue);

            Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(lambdaBody, parameter);

            return q.Where(predicate);
        }
        /// <summary>
        /// q.Where(a => a.IsDeleted == false &amp;&amp; a.IsEnabled == true)
        /// </summary>
        /// <typeparam name="T">必须含有 IsDeleted 和 IsEnabled 属性</typeparam>
        /// <param name="q"></param>
        /// <returns></returns>
        public static IQuery<TEntity> FilterDeletedAndDisabled<TEntity>(this IQuery<TEntity> q)
        {
            return q.FilterDeleted().FilterDisabled();
        }

        public static PagedData<T> TakePageData<T>(this IQuery<T> q, Pagination page)
        {
            PagedData<T> pageData = page.ToPagedData<T>();

            pageData.Models = q.TakePage(page.Page, page.PageSize).ToList();
            pageData.TotalCount = q.Count();

            return pageData;
        }
    }
}
