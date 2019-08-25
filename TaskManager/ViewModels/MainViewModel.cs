using TaskManager.Repository;
using Utils.Core;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class MainViewModel : CoreViewModel
    {
        public MainViewModel(ICommandTreeItemViewMapper mapper, ITaskRepository taskRepository)
        {
            this.TaskContainer = new TaskContainerViewModel(mapper, taskRepository);
        }

        public TaskContainerViewModel TaskContainer { get; set; }
    }
}
