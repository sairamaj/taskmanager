using TaskManager.Repository;
using Utils.Core;
using Utils.Core.Registration;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class MainViewModel : CoreViewModel
    {
        public MainViewModel(
            IServiceLocator serviceLocator,
            ICommandTreeItemViewMapper mapper, 
            ITaskRepository taskRepository)
        {
            this.TaskContainer = new TaskContainerViewModel(serviceLocator, mapper, taskRepository);
        }

        public TaskContainerViewModel TaskContainer { get; set; }
    }
}
