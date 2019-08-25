using System.Collections.Generic;
using Autofac;
using Utils.Core.Registration;

namespace Utils.Core
{
    public static class ServiceLocatorFactory
    {
        public static IServiceLocator Create(params Module[] modules)
        {
            var instance = new ServiceLocator();
            instance.Initialize(modules);
            return instance;
        }
    }
}
