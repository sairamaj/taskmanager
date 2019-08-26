using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskViewModel : CommandTreeViewModel
    {
        public object DataContext { get; }

        public TaskViewModel(string name, string tag, object dataContext) : base(null, name, tag)
        {
            DataContext = dataContext;
        }
    }
}
