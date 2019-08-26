using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskGroupViewModel : CommandTreeViewModel
    {
        public TaskGroupViewModel(string name, string tag) : base(null, name, tag)
        {
            
        }
    }
}
