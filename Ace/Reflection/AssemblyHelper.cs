using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Linq;

namespace Ace.Reflection
{
    public static class AssemblyHelper
    {
        public static List<Assembly> LoadCompileAssemblies()
        {
            List<CompilationLibrary> libs = DependencyContext.Default.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package").ToList();

            List<Assembly> ret = new List<Assembly>();

            foreach (var lib in libs)
            {

                //try
                //{
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                ret.Add(assembly);
                //}
                //catch
                //{
                //assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(dll.Name)));

                //if (assembly == null)
                //{
                //    throw;
                //}
                //}
            }

            return ret;
        }

        /* .net */
        //public static List<Assembly> LoadCompileAssemblies()
        //{
        //    Assembly currentAssembly = Assembly.GetExecutingAssembly();

        //    string currentAssemblyDir = Path.GetDirectoryName(currentAssembly.CodeBase.Replace("file:///", ""));

        //    var dllNames = Directory.GetFiles(currentAssemblyDir).Where(a => a.ToLower().EndsWith(".dll")).ToList();

        //    List<Assembly> assemblies = new List<Assembly>();
        //    foreach (var dllName in dllNames)
        //    {
        //        Assembly assembly = Assembly.LoadFrom(dllName);

        //        string lowerName = assembly.GetName().Name.ToLower();
        //        if (lowerName.StartsWith("system") || lowerName.StartsWith("microsoft"))
        //            continue;

        //        assemblies.Add(assembly);
        //    }

        //    return assemblies;
        //}

    }
}
