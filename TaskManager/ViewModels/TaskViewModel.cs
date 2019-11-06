using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    /// <summary>
    /// Task view model.
    /// </summary>
    internal class TaskViewModel : CommandTreeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskViewModel"/> class.
        /// </summary>
        /// <param name="name">
        /// Name of the task.
        /// </param>
        /// <param name="tag">
        /// A unique value for this view model.
        /// </param>
        /// <param name="dataContext">
        /// Data context for this view model.
        /// </param>
        public TaskViewModel(string name, string tag, object dataContext)
            : base(null, name, tag)
        {
            this.DataContext = dataContext;
        }
    }
}
