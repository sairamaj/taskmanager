﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Autofac;
using TaskManager.Model;
using Utils.Core.Diagnostics;
using Utils.Core.Registration;

namespace TaskManager.Extension
{
    /// <summary>
    /// Assembly tasks extensions.
    /// </summary>
    internal static class TaskAssemblyExtension
    {
        /// <summary>
        /// Initializes task assembly modules.
        /// </summary>
        /// <param name="assembly">
        /// A <see cref="Assembly"/> instance.
        /// </param>
        /// <param name="builder">
        /// A <see cref="ContainerBuilder"/> instance.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger"/> instance.
        /// </param>
        public static void InitializeTaskAssemblyModules(this Assembly assembly, ContainerBuilder builder, ILogger logger)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var initializeMethod = assembly.GetTypes()
                .Where(t => t.Name == "TaskInitializer")
                .SelectMany(t => t.GetMethods())
                .FirstOrDefault(m => m.IsStatic && m.Name == "Initialize");
            if (initializeMethod != null)
            {
                logger.Info($"Invoking {initializeMethod.Name} in {assembly.FullName}");
                initializeMethod.Invoke(null, new[] { builder });
            }
            else
            {
                logger.Warn($"No Initializer found in {assembly.FullName}");
            }
        }

        /// <summary>
        /// Gets view models from given assembly.
        /// </summary>
        /// <param name="assembly">
        /// A <see cref="Assembly"/>.
        /// </param>
        /// <param name="serviceLocator">
        /// A <see cref="IServiceLocator"/> instance.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> of <see cref="TaskInfo"/> found in given assembly.
        /// </returns>
        public static IEnumerable<TaskInfo> GetViewViewModels(this Assembly assembly, IServiceLocator serviceLocator, ILogger logger)
        {
            var viewPattern = "^Task(?<name>\\w+)View$";
            var viewRegex = new Regex(viewPattern);
            logger.Info($"Looking for Views in {viewPattern} {assembly}");
            var foundCount = 0;
            foreach (var viewTuple in assembly
                .GetTypes()
                .Where(t => viewRegex.IsMatch(t.Name))
                .Select(t =>
                {
                    var matches = viewRegex.Match(t.Name);
                    var taskName = matches.Groups["name"];
                    return new Tuple<string, Type>(taskName.Value, t);
                }))
            {
                foreach (var viewModel in assembly
                    .GetTypes()
                    .Where(t => t.Name == $"Task{viewTuple.Item1}ViewModel")
                    .Select(t => t))
                {
                    foundCount++;

                    UserControl taskView = null;
                    object dataModelContext = null;
                    Exception taskCreationException = null;
                    try
                    {
                        dataModelContext = viewModel.Assembly.CreateInstance(
                            viewModel.FullName,
                            true,
                            BindingFlags.Instance | BindingFlags.Public,
                            null,
                            CreateConstructorParameters(viewModel, serviceLocator),
                            CultureInfo.CurrentCulture,
                            null);
                        taskView = viewTuple.Item2.Assembly.CreateInstance(viewTuple.Item2.FullName) as UserControl;
                    }
                    catch (Exception e)
                    {
                        taskCreationException = e;
                    }

                    yield return new TaskInfo()
                    {
                        Type = taskCreationException == null ? TaskType.Task : TaskType.TaskWithError,
                        Name = viewTuple.Item1,
                        View = taskView,
                        DataContext = dataModelContext,
                        Tag = viewTuple.Item2.FullName,
                        TaskCreationException = taskCreationException,
                    };
                }
            }

            logger.Info($"{foundCount} found in {assembly}");
        }

        /// <summary>
        /// Create constructor parameters for given type.
        /// </summary>
        /// <param name="type">
        /// Type name for looking up constructor.
        /// </param>
        /// <param name="serviceLocator">
        /// A <see cref="IServiceLocator"/> for resolving the dependencies.
        /// </param>
        /// <returns>
        /// Constructor parameter values.
        /// </returns>
        private static object[] CreateConstructorParameters(Type type, IServiceLocator serviceLocator)
        {
            var constructor = type.GetConstructors().FirstOrDefault();
            if (constructor == null)
            {
                return new object[] { };
            }

            var constructorArgs = new List<object>();
            foreach (var parameter in constructor.GetParameters())
            {
                constructorArgs.Add(serviceLocator.Resolve(parameter.ParameterType));
            }

            return constructorArgs.ToArray();
        }
    }
}
