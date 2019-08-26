﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using TaskManager.Extension;
using TaskManager.Model;
using Utils.Core.Registration;
using Utils.Core.ViewModels;
using Module = Autofac.Module;

namespace TaskManager.Repository
{
    class TaskRepository : ITaskRepository
    {
        public async Task<IEnumerable<TaskInfo>> GetTasksAsync(IServiceLocator serviceLocator)
        {
            await Task.Delay(0);
            var taskList = new List<TaskInfo>();
            foreach (var assembly in GetTaskAssemblies())
            {
                try
                {
                    var taskInfo = GetTaskInfo(assembly, serviceLocator);
                    if (taskInfo != null)
                    {
                        taskList.Add(taskInfo);
                    }
                }
                catch (Exception e)
                {
                    // todo. collect and show as info.
                    MessageBox.Show(e.ToString());
                }
            }

            return taskList;
        }

        public async Task<IEnumerable<Module>> InitializeTaskModules(ContainerBuilder builder)
        {
            await Task.Delay(0);
            var taskModules = new List<Module>();
            foreach (var assembly in GetTaskAssemblies())
            {
                taskModules.AddRange(assembly.InitializeTaskAssemblyModules(builder));
            }

            return taskModules;
        }

        private TaskInfo GetTaskInfo(Assembly assembly, IServiceLocator serviceLocator)
        {
            var rootViewModel = assembly
                .GetTypes()
                .FirstOrDefault(t => t.Name == "RootViewModel");
            var rootView = assembly
                .GetTypes()
                .FirstOrDefault(t => t.Name == "RootView");
            if (rootViewModel != null && rootView != null)
            {
                var tag = assembly.FullName;

                // temporary testing for Demo1 which has 3 parameters
                // will be making it generic based on parameters.
                var constructor = rootViewModel.GetConstructors().FirstOrDefault();
                var parameterType = constructor.GetParameters()[2];
                var instance = serviceLocator.Resolve(parameterType.ParameterType);
                var name = GetTaskName(assembly.GetName().Name);
                return new TaskInfo()
                {
                    TaskViewModel = rootViewModel.Assembly.CreateInstance(
                        rootViewModel.FullName,
                        true,
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new object[] { name, tag , instance },
                        CultureInfo.CurrentCulture, null) as TaskViewModel,
                    Name = name,
                    Tag = tag,
                    View = rootView.Assembly.CreateInstance(rootView.FullName) as UserControl
                };
            }

            return null;
        }

        private IEnumerable<Assembly> GetTaskAssemblies()
        {
            var taskAssemblies = new List<Assembly>();
            foreach (var dir in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks")))
            {
                foreach (var taskAssembly in Directory.GetFiles(dir, "*Task.dll"))
                {
                    try
                    {
                        taskAssemblies.Add(Assembly.LoadFrom(taskAssembly));
                    }
                    catch (Exception e)
                    {
                        // need to get this back to caller to show the errors.
                    }
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
            var index = name.IndexOf("Task");
            if (index < 0)
            {
                return name;
            }

            return name.Substring(0, index);
        }
    }
}
