using System;
using System.Reflection;

namespace Utils.Core.Model
{
    public class RegistrationInfo
    {
        public Type InterfaceType { get; set; }
        public Type ImplementationType { get; set; }
        public Assembly Assembly { get; set; }
        public string AssemblyName { get; set; }
        public string Scope { get; set; }
    }
}
