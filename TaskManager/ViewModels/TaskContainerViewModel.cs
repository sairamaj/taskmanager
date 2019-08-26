using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TaskManager.Repository;
using Utils.Core;
using Utils.Core.Registration;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskContainerViewModel : CoreViewModel
    {
        public TaskContainerViewModel(
            IServiceLocator serviceLocator,
            ICommandTreeItemViewMapper mapper, 
            ITaskRepository taskRepository)
        {
            this.Tasks = new SafeObservableCollection<TaskViewModel>();
            var task = LoadTasks(serviceLocator, mapper, taskRepository);
        }

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        private async Task LoadTasks(
            IServiceLocator serviceLocator,
            ICommandTreeItemViewMapper mapper, 
            ITaskRepository taskRepository)
        {
            foreach (var task in await taskRepository.GetTasksAsync(serviceLocator))
            {
                this.Tasks.Add(task.TaskViewModel);
                mapper.Add(task.Tag, task.View);
            }
        }
    }
}
