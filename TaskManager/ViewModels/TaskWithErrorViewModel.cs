using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    /// <summary>
    /// Task with error view model.
    /// </summary>
    internal class TaskWithErrorViewModel : TaskViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskWithErrorViewModel"/> class.
        /// </summary>
        /// <param name="name">
        /// Task name.
        /// </param>
        /// <param name="tag">
        /// A unique value for this view model.
        /// </param>
        /// <param name="errorMessage">
        /// Task Error message.
        /// </param>
        public TaskWithErrorViewModel(string name, string tag, string errorMessage)
            : base(name, tag, null)
        {
            this.ErrorMessage = errorMessage;
            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
