using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskWithErrorViewModel : TaskViewModel
    {
        public TaskWithErrorViewModel(string name, string tag, string errorMessage) : base(name, tag, null)
        {
            this.ErrorMessage = errorMessage;
            this.DataContext = this;
        }

        public string ErrorMessage { get; set; }
    }
}
