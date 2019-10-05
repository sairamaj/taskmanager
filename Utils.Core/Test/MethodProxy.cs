using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

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
                return asm.GetTypes()
                    .Where(t => t.FullName == parts[0])
                    .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public));
            });

            methods.ToList().ForEach(m => Console.WriteLine(m.Name));

            _methods = methods.ToDictionary(m => m.Name, m => m, StringComparer.CurrentCultureIgnoreCase);
            _methods.ToList().ForEach(m =>
            {
                Console.WriteLine($"|{m.Key}| {m.Value}");
            });
        }

        public object Execute(string name, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"method name cannot be empty or null", nameof(name));
            }

            if (!_methods.TryGetValue(name, out var foundMethod))
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
                var index = 0;
                Console.WriteLine($"{foundMethod.Name}");
                foreach (var p in foundMethod.GetParameters())
                {
                    Console.WriteLine($"\t{p.Name} {methodInputs[index]}");
                    index++;
                }
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
