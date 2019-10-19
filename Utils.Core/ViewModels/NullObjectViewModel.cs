namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The error info view model.
    /// </summary>
    public class NullObjectViewModel : TreeViewItemViewModel
    {
        public NullObjectViewModel(string name)
            : base(null, name, true)
        {
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>Message string.</value>
        public string Message => "null";
    }

}
