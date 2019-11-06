using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Model;
using TaskManager.Repository;
using Utils.Core;
using Utils.Core.Diagnostics;
using Utils.Core.Registration;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    /// <summary>
    /// Task container view model.
    /// </summary>
    internal class TaskContainerViewModel : CoreViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskContainerViewModel"/> class.
        /// </summary>
        /// <param name="serviceLocator">
        /// A <see cref="IServiceLocator"/> instance used for resolving dependencies.
        /// </param>
        /// <param name="mapper">
        /// A <see cref="ICommandTreeItemViewMapper"/> mapper for mapping commands to views.
        /// </param>
        /// <param name="taskRepository">
        /// A <see cref="ITaskRepository"/> repository.
        /// </param>
        public TaskContainerViewModel(
            IServiceLocator serviceLocator,
            ICommandTreeItemViewMapper mapper,
            ITaskRepository taskRepository)
        {
            this.Tasks = new SafeObservableCollection<TaskViewModel>();
            var task = this.LoadTasks(serviceLocator, mapper, taskRepository);
        }

        /// <summary>
        /// Gets or sets tasks collections.
        /// </summary>
        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        /// <summary>
        /// Load tasks.
        /// </summary>
        /// <param name="serviceLocator">
        /// A <see cref="IServiceLocator"/> instance used for resolving dependencies.
        /// </param>
        /// <param name="mapper">
        /// A <see cref="ICommandTreeItemViewMapper"/> mapper for mapping commands to views.
        /// </param>
        /// <param name="taskRepository">
        /// A <see cref="ITaskRepository"/> repository.
        /// </param>
        /// <returns>
        /// A <see cref="System.Threading.Tasks.Task"/> instance.
        /// </returns>
        private async Task LoadTasks(
            IServiceLocator serviceLocator,
            ICommandTreeItemViewMapper mapper,
            ITaskRepository taskRepository)
        {
            foreach (var task in await taskRepository.GetTasksAsync(serviceLocator, serviceLocator.Resolve<ILogger>()))
            {
                if (task.Type == TaskType.TaskGroup && task.Tasks.Any())
                {
                    this.Tasks.Add(new TaskGroupViewModel(mapper, task.Name, task.Tag, task.Tasks));
                    mapper.Add(task.Tag, task.View);
                }
            }
        }
    }
}
