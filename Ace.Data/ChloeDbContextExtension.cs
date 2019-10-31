using Ace;
using Ace.AutoMapper;
using Ace.IdStrategy;
using AutoMapper;
using Chloe.Descriptors;
using Chloe.Exceptions;
using Chloe.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chloe
{
    public static class ChloeDbContextExtension
    {
        /// <summary>
        /// dbContext.Update&lt;TEntity&gt;(a => a.PrimaryKey == key, a => new TEntity() { IsEnabled = false });
        /// </summary>
        /// <typeparam name="TEntity">必须包含 IsEnabled 属性</typeparam>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int Disable<TEntity>(this IDbContext dbContext, object key)
        {
            return UpdateIsEnabled<TEntity>(dbContext, key, false);
        }
        /// <summary>
        /// dbContext.Update&lt;TEntity&gt;(a => a.PrimaryKey == key, a => new TEntity() { IsEnabled = true });
        /// </summary>
        /// <typeparam name="TEntity">必须包含 IsEnabled 属性</typeparam>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int Enable<TEntity>(this IDbContext dbContext, object key)
        {
            return UpdateIsEnabled<TEntity>(dbContext, key, true);
        }
        /// <summary>
        /// dbContext.Update&lt;TEntity&gt;(a => a.PrimaryKey == key, a => new TEntity() { IsDeleted = true, DeletionTime = deletionTime });
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <param name="deletionTime">传 null 则会将 DateTime.Now 值更新至数据库</param>
        /// <returns></returns>
        public static int SoftDelete<TEntity>(this IDbContext dbContext, object key, DateTime? deletionTime = null)
        {
            Type entityType = typeof(TEntity);

            List<MemberBinding> bindings = new List<MemberBinding>();
            bindings.Add(GetMemberAssignment(entityType, "IsDeleted", true, false));
            bindings.Add(GetMemberAssignment(entityType, "DeletionTime", deletionTime ?? DateTime.Now));

            return dbContext.Update<TEntity>(key, bindings);
        }
        /// <summary>
        /// dbContext.Update&lt;TEntity&gt;(a => a.PrimaryKey == key, a => new TEntity() { IsDeleted = true, DeletionTime = DateTime.Now, DeleteUserId = deleteUserId });
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <param name="deleteUserId"></param>
        /// <returns></returns>
        public static int SoftDelete<TEntity>(this IDbContext dbContext, object key, object deleteUserId)
        {
            return dbContext.SoftDelete<TEntity>(key, DateTime.Now, deleteUserId);
        }
        /// <summary>
        /// dbContext.Update&lt;TEntity&gt;(a => a.PrimaryKey == key, a => new TEntity() { IsDeleted = true, DeletionTime = deletionTime, DeleteUserId = deleteUserId });
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="key"></param>
        /// <param name="deletionTime"></param>
        /// <param name="deleteUserId"></param>
        /// <returns></returns>
        public static int SoftDelete<TEntity>(this IDbContext dbContext, object key, DateTime deletionTime, object deleteUserId)
        {
            Type entityType = typeof(TEntity);

            List<MemberBinding> bindings = new List<MemberBinding>();
            bindings.Add(GetMemberAssignment(entityType, "IsDeleted", true, false));
            bindings.Add(GetMemberAssignment(entityType, "DeletionTime", deletionTime));
            bindings.Add(GetMemberAssignment(entityType, "DeleteUserId", deleteUserId));

            return dbContext.Update<TEntity>(key, bindings);
        }

        /// <summary>
        /// 传入一个 dto 对象插入数据。dto 需要与实体建立映射关系，否则会报错
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static TEntity InsertFromDto<TEntity, TDto>(this IDbContext dbContext, TDto dto)
        {
            /*
             * 支持自动设置 主键=guid
             * 支持自动设置 CreationTime=DateTime.Now
             * 支持自动设置 IsDeleted=false
             */

            Utils.CheckNull(dto);

            TEntity entity = AceMapper.Map<TEntity>(dto);

            Type entityType = typeof(TEntity);
            TypeDescriptor typeDescriptor = EntityTypeContainer.GetDescriptor(entityType);

            /* 设置 主键=guid */
            if (typeDescriptor.PrimaryKeys.Count < 2) /* 只有无主键或单一主键的时候 */
            {
                var primaryKeyDescriptor = typeDescriptor.PrimaryKeys.FirstOrDefault();
                if (primaryKeyDescriptor != null && primaryKeyDescriptor.IsAutoIncrement == false)
                {
                    var keyValue = primaryKeyDescriptor.GetValue(entity);
                    if (keyValue.IsDefaultValueOfType(primaryKeyDescriptor.PropertyType) || string.Empty.Equals(keyValue))
                    {
                        /* 如果未设置主键值，则自动设置为 guid */
                        if (primaryKeyDescriptor.PropertyType == typeof(string))
                        {
                            primaryKeyDescriptor.SetValue(entity, IdHelper.CreateSnowflakeId().ToString());
                        }
                        else if (primaryKeyDescriptor.PropertyType.GetUnderlyingType() == typeof(Guid))
                        {
                            primaryKeyDescriptor.SetValue(entity, Guid.NewGuid());
                        }
                    }
                }
            }


            /* 设置 CreationTime=DateTime.Now */
            SetValueIfNeeded(entity, typeDescriptor, "CreationTime", DateTime.Now);

            /* 设置 IsDeleted=false */
            SetValueIfNeeded(entity, typeDescriptor, "IsDeleted", false);

            return dbContext.Insert(entity);
        }

        /// <summary>
        /// 传入一个 dto 对象更新数据。dto 需要与实体建立映射关系，否则会报错
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dbContext"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static int UpdateFromDto<TEntity, TDto>(this IDbContext dbContext, TDto dto)
        {
            /*
             * 动态构造 lambda 更新数据：
             * dbContext.Update<TEntity>(a => a.Id = dto.Id, a => new TEntity(){ Name = dto.Name, Age = dto.Age... });
             * 支持自动设置更新时间 UpdationTime=DateTime.Now
             */

            Utils.CheckNull(dto);

            Type entityType = typeof(TEntity);
            TypeDescriptor typeDescriptor = EntityTypeContainer.GetDescriptor(entityType);

            if (typeDescriptor.PrimaryKeys.Count != 1)
            {
                throw new NotSupportedException("仅支持只有一个主键的实体");
            }

            TypeMap typeMap = GetTypeMap<TDto, TEntity>();
            PropertyMap[] propertyMaps = typeMap.GetPropertyMaps();

            object key = null;
            List<MemberBinding> bindings = new List<MemberBinding>();

            ConstantExpression dtoConstantExp = Expression.Constant(dto);
            foreach (PropertyMap propertyMap in propertyMaps)
            {
                var mappingMemberDescriptor = typeDescriptor.TryGetPropertyDescriptor(propertyMap.DestinationProperty);

                if (mappingMemberDescriptor == null)
                    continue;

                /* 跳过主键和自增列 */
                if (mappingMemberDescriptor.IsPrimaryKey)
                {
                    key = propertyMap.SourceMember.GetMemberValue(dto);
                    continue;
                }
                if (mappingMemberDescriptor.IsAutoIncrement)
                    continue;

                MemberAssignment bind = null;
                if (propertyMap.DestinationProperty.Name == "UpdationTime")
                {
                    object updationTime = propertyMap.SourceMember.GetMemberValue(dto);
                    if (updationTime.IsDefaultValueOfType(propertyMap.SourceMember.GetMemberType())) /* 表示未设置更新时间，此时我们使用 DateTime.Now */
                    {
                        var updationTimeExp = Chloe.Extensions.ExpressionExtension.MakeWrapperAccess(DateTime.Now, propertyMap.DestinationProperty.GetMemberType());
                        bind = Expression.Bind(propertyMap.DestinationProperty, updationTimeExp);
                    }
                }

                if (bind == null)
                {
                    Expression dtoMemberAccess = Expression.MakeMemberAccess(dtoConstantExp, propertyMap.SourceMember);
                    if (propertyMap.DestinationProperty.GetMemberType() != propertyMap.SourceMember.GetMemberType())
                    {
                        dtoMemberAccess = Expression.Convert(dtoMemberAccess, propertyMap.DestinationProperty.GetMemberType());
                    }
                    bind = Expression.Bind(propertyMap.DestinationProperty, dtoMemberAccess);
                }

                bindings.Add(bind);
            }

            if (key == null)
            {
                throw new ArgumentException("未能从 dto 中找到主键或主键为空");
            }

            return dbContext.Update<TEntity>(key, bindings);
        }

        static void SetValueIfNeeded(object entity, TypeDescriptor typeDescriptor, string propertyName, object valueToSet)
        {
            PropertyInfo pi = typeDescriptor.Definition.Type.GetProperty(propertyName);
            if (pi != null && pi.PropertyType.GetUnderlyingType() == valueToSet.GetType())
            {
                var mappingMemberDescriptor = typeDescriptor.TryGetPropertyDescriptor(pi);
                if (mappingMemberDescriptor != null)
                {
                    var value = mappingMemberDescriptor.GetValue(entity);
                    if (value.IsDefaultValueOfType(mappingMemberDescriptor.PropertyType))
                    {
                        mappingMemberDescriptor.SetValue(entity, valueToSet);
                    }
                }
            }
        }
        static TypeMap GetTypeMap<TSource, TDestination>()
        {
            TypeMap typeMap = AutoMapper.Mapper.Configuration.FindTypeMapFor<TSource, TDestination>();

            if (typeMap == null)
            {
                throw new ArgumentException(string.Format("类型 '{0}' 未与类型 '{1}' 建立映射关系", typeof(TSource).FullName, typeof(TDestination).FullName));
            }

            return typeMap;
        }

        static Expression<Func<TEntity, bool>> BuildPredicate<TEntity>(object key)
        {
            /*
             * key:
             * 如果实体是单一主键，则传入的 key 与主键属性类型相同的值
             * 如果实体是多主键，则传入的 key 须是包含了与实体主键类型相同的属性的对象，如：new { Key1 = "1", Key2 = "2" }
             */

            Utils.CheckNull(key);

            Type entityType = typeof(TEntity);
            TypeDescriptor typeDescriptor = EntityTypeContainer.GetDescriptor(entityType);
            EnsureEntityHasPrimaryKey(typeDescriptor);

            KeyValuePairList<MemberInfo, object> keyValueMap = new KeyValuePairList<MemberInfo, object>();

            if (typeDescriptor.PrimaryKeys.Count == 1)
            {
                keyValueMap.Add(typeDescriptor.PrimaryKeys[0].Property, key);
            }
            else
            {
                /*
                 * key: new { Key1 = "1", Key2 = "2" }
                 */

                object multipleKeyObject = key;
                Type multipleKeyObjectType = multipleKeyObject.GetType();

                for (int i = 0; i < typeDescriptor.PrimaryKeys.Count; i++)
                {
                    var keyMemberDescriptor = typeDescriptor.PrimaryKeys[i];
                    MemberInfo keyMember = multipleKeyObjectType.GetProperty(keyMemberDescriptor.Property.Name);
                    if (keyMember == null)
                        throw new ArgumentException(string.Format("The input object does not define property for key '{0}'.", keyMemberDescriptor.Property.Name));

                    object value = keyMember.GetMemberValue(multipleKeyObject);
                    if (value == null)
                        throw new ArgumentException(string.Format("The primary key '{0}' could not be null.", keyMemberDescriptor.Property.Name));

                    keyValueMap.Add(keyMemberDescriptor.Property, value);
                }
            }

            ParameterExpression parameter = Expression.Parameter(entityType, "a");
            Expression lambdaBody = null;

            foreach (var keyValue in keyValueMap)
            {
                Expression propOrField = Expression.PropertyOrField(parameter, keyValue.Key.Name);
                Expression wrappedValue = Chloe.Extensions.ExpressionExtension.MakeWrapperAccess(keyValue.Value, keyValue.Key.GetMemberType());
                Expression e = Expression.Equal(propOrField, wrappedValue);
                lambdaBody = lambdaBody == null ? e : Expression.AndAlso(lambdaBody, e);
            }

            Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(lambdaBody, parameter);

            return predicate;
        }
        static int UpdateIsEnabled<TEntity>(IDbContext dbContext, object key, bool isEnabled)
        {
            Type entityType = typeof(TEntity);

            List<MemberBinding> bindings = new List<MemberBinding>();
            bindings.Add(GetMemberAssignment(entityType, "IsEnabled", isEnabled, false));

            return dbContext.Update<TEntity>(key, bindings);
        }
        static int Update<TEntity>(this IDbContext dbContext, object key, List<MemberBinding> bindings)
        {
            Type entityType = typeof(TEntity);
            NewExpression newExp = Expression.New(entityType);

            ParameterExpression parameter = Expression.Parameter(entityType, "a");
            Expression lambdaBody = Expression.MemberInit(newExp, bindings);

            Expression<Func<TEntity, TEntity>> lambda = Expression.Lambda<Func<TEntity, TEntity>>(lambdaBody, parameter);
            Expression<Func<TEntity, bool>> condition = BuildPredicate<TEntity>(key);

            return dbContext.Update(condition, lambda);
        }

        static void ThrowIfPropIsNull(Type entityType, PropertyInfo prop, string propName)
        {
            if (prop == null)
                throw new ArgumentException(string.Format("The type '{0}' doesn't define property '{1}'", entityType.FullName, propName));
        }
        static MemberAssignment GetMemberAssignment(Type entityType, string propName, object bindValue, bool makeWrapperAccess = true)
        {
            PropertyInfo prop = entityType.GetProperty(propName);
            ThrowIfPropIsNull(entityType, prop, propName);

            Expression exp = makeWrapperAccess ? Chloe.Extensions.ExpressionExtension.MakeWrapperAccess(bindValue, prop.PropertyType) : Expression.Constant(bindValue, prop.PropertyType);

            MemberAssignment bind = Expression.Bind(prop, exp);

            return bind;
        }

        static void EnsureEntityHasPrimaryKey(TypeDescriptor typeDescriptor)
        {
            if (!typeDescriptor.HasPrimaryKey())
                throw new ChloeException(string.Format("The entity type '{0}' does not define any primary key.", typeDescriptor.Definition.Type.FullName));
        }
    }
}
