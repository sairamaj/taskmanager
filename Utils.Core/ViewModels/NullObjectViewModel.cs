namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The error info view model.
    /// </summary>
    public class NullObjectViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectViewModel"/> class.
        /// </summary>
        /// <param name="name">
        /// Name of the view model.
        /// </param>
        public NullObjectViewModel(string name)
            : base(null, name, true)
        {
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>Message string.</value>
        public string Message => "null";
    }
}