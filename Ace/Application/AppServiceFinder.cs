using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Runtime.Loader;
using System.IO;
using Microsoft.Extensions.DependencyModel;
using Ace.Reflection;

namespace Ace.Application
{
    public static class AppServiceTypeFinder
    {
        public static List<Type> FindFromDirectory(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                return new List<Type>();
            }

            var folder = new DirectoryInfo(path);
            FileSystemInfo[] dlls = folder.GetFileSystemInfos("*.dll", SearchOption.TopDirectoryOnly);

            List<Type> ret = new List<Type>();

            foreach (var dll in dlls)
            {
                string lowerName = dll.Name.ToLower();
                if (lowerName.StartsWith("system") || lowerName.StartsWith("microsoft"))
                    continue;

                Assembly assembly;
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll.FullName);
                }
                catch (FileLoadException)
                {
                    assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(dll.Name)));

                    if (assembly == null)
                    {
                        throw;
                    }
                }

                ret.AddRange(Find(assembly));
            }

            return ret;
        }
        public static List<Type> FindFromCompileLibraries()
        {
            List<Type> ret = new List<Type>();
            List<Assembly> compileAssemblies = AssemblyHelper.LoadCompileAssemblies();
            foreach (var assembly in compileAssemblies)
            {
                ret.AddRange(Find(assembly));
            }

            return ret;
        }
        public static List<Type> Find(Assembly assembly)
        {
            IEnumerable<Type> allTypes = assembly.GetTypes();

            allTypes = allTypes.Where(a =>
            {
                var b = a.IsAbstract == false && a.IsClass && typeof(IAppService).IsAssignableFrom(a);
                return b;
            });

            List<Type> ret = allTypes.ToList();
            return ret;
        }
        public static List<Type> Find(List<Assembly> assemblies)
        {
            List<Type> ret = new List<Type>();

            foreach (var assembly in assemblies)
            {
                ret.AddRange(Find(assembly));
            }

            return ret;
        }
    }
}
