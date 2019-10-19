using System;
using System.Collections.Generic;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    class MethodInputViewModel : TreeViewItemViewModel
    {
        private readonly IDictionary<string, object> _inputs;

        public MethodInputViewModel(IDictionary<string, object> inputs) : base(null, "Inputs", true)
        {
            _inputs = inputs;
            IsExpanded = true;
        }

        protected override void LoadChildren()
        {
            foreach (var input in this._inputs)
            {
                this.Children.Add(new ObjectTreeViewModel(null, input.Key, input.Value, ObjectTreeViewModel.InfoType.Properties));
            }
        }
    }
}
