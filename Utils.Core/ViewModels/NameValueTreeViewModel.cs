namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Name value tree view model. 
    /// </summary>
    public class NameValueTreeViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValueTreeViewModel"/> class.
        /// </summary>
        /// <param name="parent">Parent tree view item.</param>
        /// <param name="name">Name of the node.</param>
        /// <param name="value">Value of the node.</param>
        public NameValueTreeViewModel(TreeViewItemViewModel parent, string name, string value)
            : base(parent, name, true)
        {
            this.Value = value;
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        /// <value>Value property.</value>
        public string Value { get; private set; }

        /// <summary>
        /// Load children.
        /// </summary>
        protected override void LoadChildren()
        {
            this.Children.Clear();
        }
    }
}