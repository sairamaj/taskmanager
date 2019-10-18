using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Utils.Core.Test
{
    class MethodProxy
    {
        private readonly Action<string> _traceInfo;
        private readonly IDictionary<string, MethodInfo> _methods;
        public MethodProxy(IEnumerable<string> typeInfo, Action<string> traceInfo)
        {
            _traceInfo = traceInfo ?? throw new ArgumentNullException(nameof(traceInfo));
            IEnumerable<MethodInfo> methods;
            if (typeInfo == null)
            {
                // load the current assembly
                methods = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !(a.FullName.Contains("System") || a.FullName.Contains("mscorlib")))
                    .SelectMany(a => a.GetTypes()).SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public));
            }
            else
            {
                methods = typeInfo.SelectMany(info =>
                {
                    var parts = info.Split(',');
                    if (parts.Length < 2)
                    {
                        // load the current assembly
                        return AppDomain.CurrentDomain.GetAssemblies()
                            .Where(a => !(a.FullName.Contains("System") || a.FullName.Contains("mscorlib")))
                            .SelectMany(a => a.GetTypes())
                            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public));
                    }
                    else
                    {
                        var asm = Assembly.Load(parts[1]);
                        return asm.GetTypes()
                            .Where(t => t.FullName == parts[0])
                            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public));
                    }
                }).ToList();
            }

            methods = methods.Union(typeof(BuiltinHelperType).GetMethods());
            _methods = new Dictionary<string, MethodInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var m in methods.OrderBy(m => m.Name))
            {
                _methods[m.Name] = m;
                _methods[$"{m.ReflectedType?.Name}.{m.Name}"] = m;
            }
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

            var argIndex = 0;
            var methodInputs = foundMethod.GetParameters().Select(p =>
            {
                argIndex++;
                object val = null;
                if (!parameters.TryGetValue(p.Name, out val))           // try named parameter
                {
                    if (!parameters.TryGetValue($"arg{argIndex}", out val))  // try positional parameter
                    {
                        return null;
                    }
                }

                if (p.ParameterType.IsPrimitive || p.ParameterType == typeof(string))
                {
                    parameters[p.Name] = Convert.ChangeType(val, p.ParameterType);
                    return parameters[p.Name];
                }
                if (p.ParameterType == typeof(Guid))
                {
                    return Guid.Parse(val?.ToString());
                }

                return JsonConvert.DeserializeObject(parameters[p.Name]?.ToString(), p.ParameterType);
            }).ToArray();

            try
            {
                var index = 0;
                var traceMessage = $"{foundMethod.Name} ";
                foreach (var p in foundMethod.GetParameters())
                {
                    traceMessage += $"\t{p.Name} {methodInputs[index]} ";
                    index++;
                }

                _traceInfo(traceMessage);
                return foundMethod.Invoke(null, methodInputs);
            }
            catch (TargetInvocationException te)
            {
                if (te.InnerException != null)
                {
                    throw te.InnerException;
                }

                if (te.InnerException is NullReferenceException exception)
                {
                    // Lets get the call stack here.
                    throw new Exception(exception.ToString(), exception.InnerException);
                }

                throw;
            }

        }
    }
}
