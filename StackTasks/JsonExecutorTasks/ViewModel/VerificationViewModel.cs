using Utils.Core.Test;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    class VerificationViewModel : TreeViewItemViewModel
    {
        private readonly ExecuteTraceInfo _traceInfo;

        public VerificationViewModel(ExecuteTraceInfo traceInfo) :base(null, $"Verify-{traceInfo.TestInfo.Name}", true)
        {
            _traceInfo = traceInfo;
            IsExpanded = true;
        }

        protected override void LoadChildren()
        {
            this.Children.Add(new ObjectTreeViewModel(null, "actual", this._traceInfo.Actual, ObjectTreeViewModel.InfoType.Properties));
            this.Children.Add(new ObjectTreeViewModel(null, "expected", this._traceInfo.Expected, ObjectTreeViewModel.InfoType.Properties));
        }
    }
}
