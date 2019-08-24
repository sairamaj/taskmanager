using System;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The child changed event args.
    /// </summary>
    public class ChildChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChildChangedEventArgs"/> class.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public ChildChangedEventArgs(TreeViewItemViewModel item)
        {
            this.Item = item;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>Instance of <see cref="TreeViewItemViewModel"/> class.</value>
        public TreeViewItemViewModel Item { get; private set; }
    }
}
