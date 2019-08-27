using TaskManager.Repository;
using Utils.Core;
using Utils.Core.Diagnostics;
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
            this.Logger = serviceLocator.Resolve<ILogger>();
        }

        public TaskContainerViewModel TaskContainer { get; set; }
        public ILogger Logger { get; private set; }
    }
}
