using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    class MethodOutputViewModel : TreeViewItemViewModel
    {
        private readonly object _output;

        public MethodOutputViewModel(object output) : base(null, "Return", true)
        {
            _output = output;
            IsExpanded = true;
        }

        protected override void LoadChildren()
        {
            this.Children.Add(new ObjectTreeViewModel(null, "return", _output, ObjectTreeViewModel.InfoType.Properties));
        }
    }
}
