using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskViewModel : CommandTreeViewModel
    {
        public TaskViewModel(string name) : base(null, name, name)
        {
        }
    }
}
