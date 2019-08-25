using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Model;
using Utils.Core.ViewModels;

namespace TaskManager.Repository
{
    class TaskRepository : ITaskRepository
    {
        public async Task<IEnumerable<TaskInfo>> GetTasksAsync()
        {
            await Task.Delay(0);
            var taskList = new List<TaskInfo>();
            foreach (var dir in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks")))
            {
                foreach (var taskAssembly in Directory.GetFiles(dir, "*Task.dll"))
                {
                    try
                    {
                        var asm = Assembly.LoadFrom(taskAssembly);
                        var taskInfo = GetTaskInfo(asm);
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
            }

            return taskList;
        }

        private TaskInfo GetTaskInfo(Assembly assembly)
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
                return new TaskInfo()
                {
                    TaskViewModel = rootViewModel.Assembly.CreateInstance(
                        rootViewModel.FullName,
                        true,
                        BindingFlags.Instance | BindingFlags.Public,
                        null,
                        new object[] { assembly.GetName().Name, tag },
                        CultureInfo.CurrentCulture, null) as TaskViewModel,
                    Name = assembly.GetName().Name,
                    Tag = tag,
                    View = rootView.Assembly.CreateInstance(rootView.FullName) as UserControl
                };
            }

            return null;
        }
    }
}
