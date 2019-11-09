using Utils.Core.Test;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    /// <summary>
    /// Method view tree item.
    /// </summary>
    internal class MethodViewTreeViewModel : TreeViewItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodViewTreeViewModel"/> class.
        /// </summary>
        /// <param name="executeTrace">
        /// Execution trace info.
        /// </param>
        public MethodViewTreeViewModel(ExecuteTraceInfo executeTrace)
            : base(null, executeTrace.MethodName, true)
        {
            this.ExecuteTrace = executeTrace;
            this.IsExpanded = true;
        }

        /// <summary>
        /// Gets execution trace.
        /// </summary>
        public ExecuteTraceInfo ExecuteTrace { get; }

        /// <summary>
        /// Loads the method inputs and output tree items.
        /// </summary>
        protected override void LoadChildren()
        {
            this.Children.Add(new MethodInputViewModel(this.ExecuteTrace.MethodParameters));
            if (this.ExecuteTrace.MethodReturnType != null )
            {
                if (this.ExecuteTrace.MethodReturnValue != null)
                {
                    this.Children.Add(new MethodOutputViewModel(this.ExecuteTrace.MethodReturnValue));
                }
                else
                {
                    this.Children.Add(new NullObjectViewModel(this.ExecuteTrace.MethodReturnType?.ToString()));
                }
            }

            if (this.ExecuteTrace.MethodException != null)
            {
                this.Children.Add(new ExceptionTreeViewItemViewModel(this, this.ExecuteTrace.MethodException));
            }
        }
    }
}
