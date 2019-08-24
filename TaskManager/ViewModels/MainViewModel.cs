using Utils.Core;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class MainViewModel : CoreViewModel
    {
        public MainViewModel(ICommandTreeItemViewMapper mapper)
        {
            this.TaskContainer = new TaskContainerViewModel(mapper);
        }

        public TaskContainerViewModel TaskContainer { get; set; }
    }
}
