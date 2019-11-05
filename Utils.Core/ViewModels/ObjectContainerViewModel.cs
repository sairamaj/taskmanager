namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Object container view model.
    /// </summary>
    public class ObjectContainerViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Object value.
        /// </summary>
        private readonly object _obj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectContainerViewModel"/> class.
        /// </summary>
        /// <param name="parent">
        /// Parent view model.
        /// </param>
        /// <param name="obj">
        /// Object to be displayed.
        /// </param>
        /// <param name="name">
        /// Name of the view model.
        /// </param>
        public ObjectContainerViewModel(TreeViewItemViewModel parent, object obj, string name)
            : base(parent, name, true)
        {
            this._obj = obj;
        }
    }
}
