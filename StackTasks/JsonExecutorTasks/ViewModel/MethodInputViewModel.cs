using System;
using System.Collections.Generic;
using Utils.Core.Model;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    /// <summary>
    /// Method input view model.
    /// </summary>
    internal class MethodInputViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// method inputs.
        /// </summary>
        private readonly IDictionary<string, object> _inputs;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInputViewModel"/> class.
        /// </summary>
        /// <param name="inputs">
        /// Method inputs.
        /// </param>
        public MethodInputViewModel(IDictionary<string, object> inputs)
            : base(null, "Inputs", true)
        {
            this._inputs = inputs;
            this.IsExpanded = true;
        }

        /// <summary>
        /// Load inputs.
        /// </summary>
        protected override void LoadChildren()
        {
            foreach (var input in this._inputs)
            {
                this.Children.Add(new ObjectTreeViewModel(null, input.Key, input.Value, InfoType.Properties));
            }
        }
    }
}
