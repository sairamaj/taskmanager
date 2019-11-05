using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Utils.Core.Test
{
    /// <summary>
    /// Proxy for method.
    /// </summary>
    internal class MethodProxy
    {
        /// <summary>
        /// Trace info.
        /// </summary>
        private readonly Action<ExecuteTraceInfo> _traceInfo;

        /// <summary>
        /// Methods list.
        /// </summary>
        private readonly IDictionary<string, MethodInfo> _methods;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodProxy"/> class.
        /// </summary>
        /// <param name="typeInfo">
        /// Type information fromw which methods needs to be parsed.
        /// </param>
        /// <param name="traceInfo">
        /// Trace info emitter action.
        /// </param>
        public MethodProxy(IEnumerable<string> typeInfo, Action<ExecuteTraceInfo> traceInfo)
        {
            this._traceInfo = traceInfo ?? throw new ArgumentNullException(nameof(traceInfo));
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

                    var asm = Assembly.Load(parts[1]);
                    return asm.GetTypes()
                        .Where(t => t.FullName == parts[0])
                        .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public));
                }).ToList();
            }

            methods = methods.Union(typeof(BuiltinHelperType).GetMethods());
            this._methods = new Dictionary<string, MethodInfo>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var m in methods.OrderBy(m => m.Name))
            {
                this._methods[m.Name] = m;
                this._methods[$"{m.ReflectedType?.Name}.{m.Name}"] = m;
            }
        }

        /// <summary>
        /// Gets or sets return type of the method executed.
        /// </summary>
        public Type ReturnType { get; set; }

        /// <summary>
        /// Executes method.
        /// </summary>
        /// <param name="name">
        /// Method name.
        /// </param>
        /// <param name="parameters">
        /// Method parameters.
        /// </param>
        /// <returns>
        /// Return value from method execution.
        /// </returns>
        public object Execute(string name, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"method name cannot be empty or null", nameof(name));
            }

            if (!this._methods.TryGetValue(name, out var foundMethod))
            {
                throw new Exception($"Method {name} not found.");
            }

            this.ReturnType = foundMethod.ReturnType;   // todo need to move to ctor
            var argIndex = 0;
            var methodInputs = foundMethod.GetParameters().Select(p =>
            {
                argIndex++;
                object val = null;

                // try named parameter
                if (!parameters.TryGetValue(p.Name, out val))
                {
                    // try positional parameter
                    if (!parameters.TryGetValue($"arg{argIndex}", out val))
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

                if (p.ParameterType == typeof(IDictionary<string, object>))
                {
                    return parameters[p.Name];
                }

                return JsonConvert.DeserializeObject(parameters[p.Name]?.ToString(), p.ParameterType);
            }).ToArray();

            object returnValue = null;
            Exception methodException = null;
            IDictionary<string, object> inputs = null;
            try
            {
                var index = 0;
                inputs = foundMethod.GetParameters().ToDictionary(p => p.Name, p => methodInputs[index++]);
                returnValue = foundMethod.Invoke(null, methodInputs);

                return returnValue;
            }
            catch (TargetInvocationException te)
            {
                if (te.InnerException != null)
                {
                    methodException = te.InnerException;
                    if (te.InnerException is NullReferenceException exception)
                    {
                        // Lets get the call stack here.
                        throw new Exception(exception.ToString(), exception.InnerException);
                    }

                    throw te.InnerException;
                }

                throw;
            }
            finally
            {
                this._traceInfo(new ExecuteTraceInfo(TraceType.MethodFinished)
                {
                    MethodName = foundMethod.Name,
                    MethodReturnValue = returnValue,
                    MethodParameters = inputs,
                    MethodException = methodException,
                    MethodReturnType = foundMethod.ReturnType,
                });
            }
        }
    }
}
