using System.Collections.Generic;
using Autofac;
using Utils.Core.Registration;

namespace Utils.Core
{
    /// <summary>
    /// Service locator factory.
    /// </summary>
    public static class ServiceLocatorFactory
    {
        /// <summary>
        /// Create service locator.
        /// </summary>
        /// <param name="builder">
        /// Container builder.
        /// </param>
        /// <returns>
        /// Instance of <see cref="IServiceLocator"/>.
        /// </returns>
        public static IServiceLocator Create(ContainerBuilder builder)
        {
            var instance = new ServiceLocator();
            instance.Initialize(builder);
            return instance;
        }
    }
}
