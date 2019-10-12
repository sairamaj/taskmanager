using System.Windows.Controls;
using Utils.Core.ViewModels;

namespace JsonExecutorTasks.Views
{
    /// <summary>
    /// Interaction logic for TaskTestView.xaml
    /// </summary>
    public partial class TaskTestView : UserControl
    {
        public TaskTestView()
        {
            InitializeComponent();
            this.TestFilesContainerView.TaskSelectionChangedEvent += (s, e) =>
            {
                var viewModel = e.SelectedItem as CommandTreeViewModel;
                if (viewModel == null)
                {
                    return;
                }

                var ctrl = new TestView {DataContext = viewModel.DataContext};
                this.DetailViewContainer.ShowView(ctrl);
            };
        }
    }
}
