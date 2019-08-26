using System.Collections.Generic;
using Autofac;
using Utils.Core.Registration;

namespace Utils.Core
{
    public static class ServiceLocatorFactory
    {
        public static IServiceLocator Create(ContainerBuilder builder)
        {
            var instance = new ServiceLocator();
            instance.Initialize(builder);
            return instance;
        }
    }
}
