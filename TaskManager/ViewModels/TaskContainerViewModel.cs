using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManager.Repository;
using Utils.Core;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskContainerViewModel : CoreViewModel
    {
        public TaskContainerViewModel(ICommandTreeItemViewMapper mapper, ITaskRepository taskRepository)
        {
            this.Tasks = new SafeObservableCollection<TaskViewModel>();
            var task = LoadTasks(taskRepository);
        }

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        private async Task LoadTasks(ITaskRepository taskRepository)
        {
            foreach (var task in await taskRepository.GetTasksAsync())
            {
                this.Tasks.Add(new TaskViewModel(task.Name, task.Name));
            }
        }
    }
}
