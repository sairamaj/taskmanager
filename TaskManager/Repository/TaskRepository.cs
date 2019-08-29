using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using Autofac;
using TaskManager.Extension;
using TaskManager.Model;
using Utils.Core.Diagnostics;
using Utils.Core.Registration;
using Module = Autofac.Module;

namespace TaskManager.Repository
{
    class TaskRepository : ITaskRepository
    {
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
                    Tasks = new List<TaskInfo>()
                };

                taskList.Add(parentTask);

                var subTasks = new List<TaskInfo>();
                foreach (var assembly in GetTaskAssemblies(dir, "*Tasks.dll", logger))
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

        public async Task<IEnumerable<Module>> InitializeTaskModules(ContainerBuilder builder, ILogger logger)
        {
            await Task.Delay(0);
            var taskModules = new List<Module>();
            foreach (var assembly in GetTaskAssemblies(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*Tasks.dll", logger))
            {
                logger.Debug($"Loading {assembly.FullName}");
                try
                {
                    taskModules.AddRange(assembly.InitializeTaskAssemblyModules(builder));
                }
                catch (TargetInvocationException te)
                {
                    logger.Error($"Error Initialize {assembly.FullName} {te.InnerException}");
                }
            }

            foreach (var assembly in GetTaskAssemblies(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks"), "*Startup.dll", logger))
            {
                logger.Debug($"Loading {assembly.FullName}");
                try
                {
                    taskModules.AddRange(assembly.InitializeTaskAssemblyModules(builder));
                }
                catch (TargetInvocationException te)
                {
                    logger.Error($"Error Initialize {assembly.FullName} {te.InnerException}");
                }
            }

            return taskModules;
        }

    
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
        static string GetTaskName(string name)
        {
            if (name == null)
            {
                return null;
            }
            var index = name.IndexOf("Tasks");
            if (index < 0)
            {
                return name;
            }

            return name.Substring(0, index);
        }


    }
}