using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskViewModel : CommandTreeViewModel
    {
        public TaskViewModel(string name, string tag, object dataContext) : base(null, name, tag)
        {
            this.DataContext = dataContext;
        }
    }
}
