using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Expression<Func<TSource, bool>> And<TSource>(this Expression<Func<TSource, bool>> a, Expression<Func<TSource, bool>> b)
        {
            Type rootType = typeof(TSource);
            var memberParam = Expression.Parameter(rootType, "root");
            var aNewBody = ParameterExpressionReplacer.Replace(a.Body, memberParam);
            var bNewBody = ParameterExpressionReplacer.Replace(b.Body, memberParam);
            var newBody = Expression.And(aNewBody, bNewBody);
            var lambda = Expression.Lambda<Func<TSource, bool>>(newBody, memberParam);
            return lambda;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Expression<Func<TSource, bool>> Or<TSource>(this Expression<Func<TSource, bool>> a, Expression<Func<TSource, bool>> b)
        {
            Type rootType = typeof(TSource);
            var memberParam = Expression.Parameter(rootType, "root");
            var aNewBody = ParameterExpressionReplacer.Replace(a.Body, memberParam);
            var bNewBody = ParameterExpressionReplacer.Replace(b.Body, memberParam);
            var newBody = Expression.Or(aNewBody, bNewBody);
            var lambda = Expression.Lambda<Func<TSource, bool>>(newBody, memberParam);
            return lambda;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ParameterExpressionReplacer : ExpressionVisitor
    {
        ParameterExpression replaceWith;

        private ParameterExpressionReplacer(ParameterExpression replaceWith)
        {
            this.replaceWith = replaceWith;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static Expression Replace(Expression expression, ParameterExpression replaceWith)
        {
            return new ParameterExpressionReplacer(replaceWith).Visit(expression);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this.replaceWith;
        }
    }

}
