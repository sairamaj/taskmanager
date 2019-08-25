using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utils.Core.ViewModels;

namespace TaskManager.Repository
{
    class TaskRepository : ITaskRepository
    {
        public async Task<IEnumerable<TaskViewModel>> GetTasksAsync()
        {
            await Task.Delay(0);
            var taskList = new List<TaskViewModel>();
            foreach (var dir in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tasks")))
            {
                taskList.Add(new TaskViewModel(Path.GetFileName(dir),dir));
            }

            return taskList;
        }
    }
}
