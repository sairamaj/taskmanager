using Utils.Core.Model;
using Utils.Core.Test;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    /// <summary>
    /// Verification view model.
    /// </summary>
    internal class VerificationViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Trace information.
        /// </summary>
        private readonly ExecuteTraceInfo _traceInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationViewModel"/> class.
        /// </summary>
        /// <param name="traceInfo">
        /// Trace information.
        /// </param>
        public VerificationViewModel(ExecuteTraceInfo traceInfo)
            : base(null, $"Verify-{traceInfo.TestInfo.Name}", true)
        {
            this._traceInfo = traceInfo;
            this.IsExpanded = true;
        }

        /// <summary>
        /// Loads the verification children in tree view.
        /// </summary>
        protected override void LoadChildren()
        {
            this.Children.Add(new ObjectTreeViewModel(null, "actual", this._traceInfo.Actual, InfoType.Properties));
            this.Children.Add(new ObjectTreeViewModel(null, "expected", this._traceInfo.Expected, InfoType.Properties));
        }
    }
}