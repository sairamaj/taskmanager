using System;

namespace Utils.Core.ViewModels
{
    /// <summary>
    /// Name value view model.
    /// </summary>
    public class NameValueViewModel : CoreViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValueViewModel"/> class.
        /// </summary>
        /// <param name="name">
        /// Name.
        /// </param>
        /// <param name="value">
        /// Value.
        /// </param>
        public NameValueViewModel(string name, string value)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public string Value { get; set; }
    }
}