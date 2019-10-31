using Ace.Reflection;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Ace.AutoMapper
{
    public static class AceMapper
    {
        public static object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, destination, sourceType, destinationType);
        }
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }
        public static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }
        public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Mapper.Map<TSource, TDestination>(source, destination);
        }
        public static object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }

        public static void InitializeMap()
        {
            List<Assembly> assemblies = AssemblyHelper.LoadCompileAssemblies();
            List<MapperDescritor> mapperDescritors = FindMapperDescritors(assemblies);

            CreateMap(mapperDescritors);
        }
        public static void InitializeMap(Assembly assembly)
        {
            List<MapperDescritor> mapperDescritors = FindMapperDescritors(assembly);
            CreateMap(mapperDescritors);
        }


        static void CreateMap(List<MapperDescritor> mapperDescritors)
        {
            Mapper.Initialize(cfg =>
            {
                foreach (MapperDescritor mapperDescritor in mapperDescritors)
                {

                    IMappingExpression exp = cfg.CreateMap(mapperDescritor.SourceType, mapperDescritor.TargetType);
                    IMappingExpression reversedMapExp = null;
                    if (mapperDescritor.ReverseMap)
                    {
                        reversedMapExp = exp.ReverseMap();
                    }

                    foreach (MapperMemberRelationship memberRelationship in mapperDescritor.MemberRelationships)
                    {
                        exp.ForMember(memberRelationship.TargetMember.Name, mce => mce.MapFrom(memberRelationship.SourceMember.Name));

                        if (reversedMapExp != null)
                        {
                            reversedMapExp.ForMember(memberRelationship.SourceMember.Name, mce => mce.MapFrom(memberRelationship.TargetMember.Name));
                        }
                    }
                }
            });
        }
        static List<MapperDescritor> FindMapperDescritors(List<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => FindMapperDescritors(a)).ToList();
        }
        static List<MapperDescritor> FindMapperDescritors(Assembly assembly)
        {
            IEnumerable<Type> exportedTypes = assembly.ExportedTypes;

            List<MapperDescritor> ret = new List<MapperDescritor>();

            foreach (Type exportedType in exportedTypes)
            {
                MapperDescritor mapperDescritorOfMapToTypeAttr = FindMapperDescritorForMapToType(exportedType);
                if (mapperDescritorOfMapToTypeAttr != null)
                {
                    ret.Add(mapperDescritorOfMapToTypeAttr);
                }

                MapperDescritor mapperDescritorOfMapFromTypeAttr = FindMapperDescritorForMapFromType(exportedType);
                if (mapperDescritorOfMapFromTypeAttr != null)
                {
                    ret.Add(mapperDescritorOfMapFromTypeAttr);
                }
            }

            return ret;
        }

        static MapperDescritor FindMapperDescritorForMapToType(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            bool hasMapToTypeAttribute = typeInfo.IsDefined(typeof(MapToTypeAttribute));
            if (!hasMapToTypeAttribute)
            {
                return null;
            }

            MapToTypeAttribute mapToTypeAttribute = typeInfo.GetCustomAttribute<MapToTypeAttribute>();

            Type sourceType = type;
            Type targetType = mapToTypeAttribute.Type;
            bool reverseMap = mapToTypeAttribute.ReverseMap;

            MapperDescritor mapperDescritor = new MapperDescritor(sourceType, targetType, reverseMap);

            var properties = typeInfo.GetProperties();
            foreach (var property in properties)
            {
                if (property.IsDefined(typeof(MapToMemberAttribute)) == false)
                {
                    continue;
                }

                MapToMemberAttribute mapToMemberAttribute = property.GetCustomAttribute<MapToMemberAttribute>();

                PropertyInfo targetMember = targetType.GetProperty(mapToMemberAttribute.MemberName);
                if (targetMember == null)
                    throw new Exception(string.Format("Target type '{0}' does not define property named '{1}'.", targetType.FullName, mapToMemberAttribute.MemberName));

                MapperMemberRelationship memberRelationship = new MapperMemberRelationship(property, targetMember);

                mapperDescritor.MemberRelationships.Add(memberRelationship);
            }

            return mapperDescritor;
        }
        static MapperDescritor FindMapperDescritorForMapFromType(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            bool hasMapFromTypeAttribute = typeInfo.IsDefined(typeof(MapFromTypeAttribute));
            if (!hasMapFromTypeAttribute)
            {
                return null;
            }

            MapFromTypeAttribute mapFromTypeAttribute = typeInfo.GetCustomAttribute<MapFromTypeAttribute>();

            Type sourceType = mapFromTypeAttribute.Type;
            Type targetType = type;
            bool reverseMap = mapFromTypeAttribute.ReverseMap;

            MapperDescritor mapperDescritor = new MapperDescritor(sourceType, targetType, reverseMap);

            var properties = typeInfo.GetProperties();
            foreach (var property in properties)
            {
                if (property.IsDefined(typeof(MapFromMemberAttribute)) == false)
                {
                    continue;
                }

                MapFromMemberAttribute mapFromMemberAttribute = property.GetCustomAttribute<MapFromMemberAttribute>();

                PropertyInfo sourceMember = sourceType.GetProperty(mapFromMemberAttribute.MemberName);
                if (sourceMember == null)
                    throw new Exception(string.Format("Source type '{0}' does not define property named '{1}'.", sourceType.FullName, mapFromMemberAttribute.MemberName));

                MapperMemberRelationship memberRelationship = new MapperMemberRelationship(sourceMember, property);

                mapperDescritor.MemberRelationships.Add(memberRelationship);
            }

            return mapperDescritor;
        }

    }
}
