using Utils.Core.Test;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    class MethodTreeViewModel : TreeViewItemViewModel
    {
        public MethodTreeViewModel(ExecuteTraceInfo executeTrace) : base(null, executeTrace.MethodName, true)
        {
            ExecuteTrace = executeTrace;
            this.IsExpanded = true;
        }

        public ExecuteTraceInfo ExecuteTrace { get; }

        protected override void LoadChildren()
        {
            this.Children.Add(new MethodInputViewModel(this.ExecuteTrace.MethodParameters));
        }
    }
}
