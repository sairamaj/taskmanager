namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The command tree view model.
    /// </summary>
    public class CommandTreeViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandTreeViewModel"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="tag">
        /// The tag.
        /// </param>
        public CommandTreeViewModel(TreeViewItemViewModel parent, string name, string tag)
            : base(parent, name, true)
        {
            // IsExpanded = true;
            this.Tag = tag;
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>Tag value.</value>
        public string Tag { get; set; }

        /// <summary>
        /// The clear.
        /// </summary>
        public virtual void Clear()
        {
        }
    }
}
