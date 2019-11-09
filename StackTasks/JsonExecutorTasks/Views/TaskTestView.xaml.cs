using System.Windows.Controls;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.Views
{
    /// <summary>
    /// Interaction logic for TaskTestView.xaml.
    /// </summary>
    public partial class TaskTestView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTestView"/> class.
        /// </summary>
        public TaskTestView()
        {
            this.InitializeComponent();
            this.TestFilesContainerView.TaskSelectionChangedEvent += (s, e) =>
            {
                if (!(e.SelectedItem is CommandTreeViewModel viewModel))
                {
                    return;
                }

                var ctrl = new TestView { DataContext = viewModel.DataContext };
                this.DetailViewContainer.ShowView(ctrl);
            };
        }
    }
}
