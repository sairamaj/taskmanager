using System;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// The vie model events.
    /// </summary>
    public class ViewModelEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelEventArgs"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public ViewModelEventArgs(string name, object data)
        {
            this.Name = name;
            this.Data = data;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>Name of event.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>Data for event.</value>
        public object Data { get; private set; }
    }
}
