using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using TaskManager.Extension;
using TaskManager.Model;
using Utils.Core.Diagnostics;
using Utils.Core.Registration;
using Module = Autofac.Module;

namespace TaskManager.Repository
{
    /// <summary>
    /// Repository for getting task information.
    /// </summary>
    internal class TaskRepository : ITaskRepository
    {
        /// <summary>
        /// Gets all tasks information.
        /// </summary>
        /// <param name="serviceLocator">
        /// A <see cref="IServiceLocator"/> instance used for resolving dependencies.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> of tasks info.
        /// </returns>
        public async Task<IEnumerable<TaskInfo>> GetTasksAsync(IServiceLocator serviceLocator, ILogger logger)
        {
            await Task.Delay(0);
            var taskList = new List<TaskInfo>();
            foreach (var dir in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks")))
            {
                var parentTask = new TaskInfo
                {
                    Type = TaskType.TaskGroup,
                    Name = Path.GetFileName(dir),
                    Tag = dir,
                    Tasks = new List<TaskInfo>(),
                };

                taskList.Add(parentTask);

                var subTasks = new List<TaskInfo>();
                foreach (var assembly in this.GetTaskAssemblies(dir, "*Tasks.dll", logger))
                {
                    try
                    {
                        foreach (var taskInfo in assembly.GetViewViewModels(serviceLocator, logger))
                        {
                            subTasks.Add(taskInfo);
                        }
                    }
                    catch (Exception e)
                    {
                        logger.Error($"GetTasksAsync {e}");
                    }
                }

                parentTask.Tasks = subTasks;
            }

            return taskList;
        }

        /// <summary>
        /// Initializes task modules.
        /// </summary>
        /// <param name="builder">
        /// A <see cref="ContainerBuilder"/> instance.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger"/> instance.
        /// </param>
        /// <returns>
        /// A task object.
        /// </returns>
        public async Task InitializeTaskModulesAsync(ContainerBuilder builder, ILogger logger)
        {
            await Task.Delay(0);
            var taskModules = new List<Module>();
            foreach (var assembly in this.GetTaskAssemblies(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*Tasks.dll", logger))
            {
                logger.Debug($"Loading {assembly.FullName}");
                try
                {
                    assembly.InitializeTaskAssemblyModules(builder, logger);
                }
                catch (TargetInvocationException te)
                {
                    logger.Error($"Error Initialize {assembly.FullName} {te.InnerException}");
                }
            }

            foreach (var assembly in this.GetTaskAssemblies(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*Startup.dll", logger))
            {
                logger.Debug($"Loading {assembly.FullName}");
                try
                {
                    assembly.InitializeTaskAssemblyModules(builder, logger);
                }
                catch (TargetInvocationException te)
                {
                    logger.Error($"Error Initialize {assembly.FullName} {te.InnerException}");
                }
            }
        }

        /// <summary>
        /// Gets assemblies containing tasks.
        /// </summary>
        /// <param name="path">
        /// Path to look for assemblies.
        /// </param>
        /// <param name="filter">
        /// File Filter.
        /// </param>
        /// <param name="logger">
        /// Logger instance.
        /// </param>
        /// <returns>
        /// A list of assemblies containing tasks.
        /// </returns>
        private IEnumerable<Assembly> GetTaskAssemblies(string path, string filter, ILogger logger)
        {
            var taskAssemblies = new List<Assembly>();
            foreach (var taskAssembly in Directory.GetFiles(path, filter, SearchOption.AllDirectories))
            {
                try
                {
                    taskAssemblies.Add(Assembly.LoadFrom(taskAssembly));
                }
                catch (Exception e)
                {
                    logger.Error($"GetTaskAssemblies {e}");
                }
            }

            return taskAssemblies;
        }
    }
}