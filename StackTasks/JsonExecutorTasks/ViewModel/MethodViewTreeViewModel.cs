using System.Threading;
using Utils.Core.Test;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.ViewModel
{
    class MethodViewTreeViewModel : TreeViewItemViewModel
    {
        public MethodViewTreeViewModel(ExecuteTraceInfo executeTrace) : base(null, executeTrace.MethodName, true)
        {
            ExecuteTrace = executeTrace;
            this.IsExpanded = true;
        }

        public ExecuteTraceInfo ExecuteTrace { get; }

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
