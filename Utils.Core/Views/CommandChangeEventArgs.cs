using System;

namespace Utils.Core.Views
{
    /// <summary>
    /// Command change event arguments class.
    /// </summary>
    public class CommandChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Selected item object.
        /// </summary>
        private readonly object _selectedItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandChangeEventArgs"/> class.
        /// </summary>
        /// <param name="selectedItem">Selected item object.</param>
        public CommandChangeEventArgs(object selectedItem)
        {
            this._selectedItem = selectedItem;
        }

        /// <summary>
        /// Gets selected item.
        /// </summary>
        /// <value>Selected object.</value>
        public object SelectedItem => this._selectedItem;
    }
}
