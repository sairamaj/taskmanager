using System;
using System.Reflection;

namespace Utils.Core.Model
{
    /// <summary>
    /// Registration information.
    /// </summary>
    public class RegistrationInfo
    {
        /// <summary>
        /// Gets or sets interface type.
        /// </summary>
        public Type InterfaceType { get; set; }

        /// <summary>
        /// Gets or sets implementation type.
        /// </summary>
        public Type ImplementationType { get; set; }

        /// <summary>
        /// Gets or sets assembly name.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Gets or sets assembly name.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets lifetime scope.
        /// </summary>
        public string Scope { get; set; }
    }
}
