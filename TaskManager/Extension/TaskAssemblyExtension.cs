using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Autofac;
using Module = Autofac.Module;

namespace TaskManager.Extension
{
    internal static class TaskAssemblyExtension
    {
        public static IEnumerable<Module> InitializeTaskAssemblyModules(this Assembly assembly, ContainerBuilder builder)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var initializeMethod = assembly.GetTypes()
                .Where(t => t.Name == "TaskInitializer")
                .SelectMany(t => t.GetMethods())
                .FirstOrDefault(m => m.IsStatic && m.Name == "Initialize");
            if (initializeMethod != null)
            {
                initializeMethod.Invoke(null, new[] { builder });
            }
            return new List<Module>();
        }
    }
}
