using System.Collections.Generic;
using Autofac;
using Autofac.Core;

namespace Utils.Core.Registration
{
    public class ServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Container instance.
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// Initializes AUTOFAC container.
        /// </summary>
        public void Initialize(IEnumerable<Module> modules)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());
            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            _container = builder.Build();
        }

        /// <summary>
        /// Resolves the object from container.
        /// </summary>
        /// <typeparam name="T">Type name.</typeparam>
        /// <returns>Resolved instance.</returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// Resolves the object from container.
        /// </summary>
        /// <typeparam name="T">Type name.</typeparam>
        /// <param name="parameters">Resolution parameters.</param>
        /// <returns>Resolved instance.</returns>
        public T Resolve<T>(params Parameter[] parameters)
        {
            return _container.Resolve<T>(parameters);
        }
    }
}
