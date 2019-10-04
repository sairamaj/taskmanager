using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Core.Test
{
    class MethodProxy
    {
        private readonly IDictionary<string, MethodInfo> _methods;
        public MethodProxy(IEnumerable<string> typeInfo)
        {
            var methods = typeInfo.SelectMany(info =>
            {
                var parts = info.Split(',');
                if (parts.Length < 2)
                {
                    throw new ConfigurationErrorsException($"TypeName should be of form type,assembly");
                }

                var asm = Assembly.Load(parts[1]);
                return asm.GetTypes().SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public));
            });

            _methods = methods.ToDictionary(m => m.Name, m => m, StringComparer.CurrentCultureIgnoreCase);
            _methods.ToList().ForEach(m =>
            {
                Console.WriteLine($"|{m.Key}| {m.Value}");
            });
        }

        public object Execute(string name, IDictionary<string, object> parameters)
        {
            if(!_methods.TryGetValue(name, out var foundMethod))
            {
                throw new Exception($"Method {name} not found.");
            }

            var methodInputs = foundMethod.GetParameters().Select(p =>
            {
                if (parameters.ContainsKey(p.Name))
                {
                    parameters[p.Name] = Convert.ChangeType(parameters[p.Name], p.ParameterType);
                    return parameters[p.Name];
                }

                return null;
            }).ToArray();

            try
            {
                return foundMethod.Invoke(null, methodInputs);
            }
            catch (TargetInvocationException te)
            {
                if (te.InnerException != null)
                {
                    throw te.InnerException;
                }

                throw;
            }

        }
    }
}
