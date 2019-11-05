using System;
using System.Collections.Generic;
using Autofac.Core;
using Utils.Core.Model;

namespace Utils.Core.Registration
{
    /// <summary>
    /// Service locator for getting services.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Resolves instance of given type.
        /// </summary>
        /// <typeparam name="T">
        /// A type name.
        /// </typeparam>
        /// <returns>
        /// Instance of given type.
        /// </returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves instance of given type.
        /// </summary>
        /// <typeparam name="T">
        /// A type name.
        /// </typeparam>
        /// <param name="parameters">
        /// Instance level parameters.
        /// </param>
        /// <returns>
        /// Instance of given type.
        /// </returns>
        T Resolve<T>(params Parameter[] parameters);

        /// <summary>
        /// Resolves instance of given type.
        /// </summary>
        /// <param name="serviceType">
        /// Service type.
        /// </param>
        /// <returns>
        /// Instance of given type.
        /// </returns>
        object Resolve(Type serviceType);

        /// <summary>
        /// Gets registrations.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> of type <see cref="RegistrationInfo"/>.
        /// </returns>
        IEnumerable<RegistrationInfo> GetRegisteredTypes();
    }
}
