using TaskManager.ViewModels;
using Utils.Core;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="mapper">
        /// A <see cref="ICommandTreeItemViewMapper"/> mapper.
        /// </param>
        public MainWindow(ICommandTreeItemViewMapper mapper)
        {
            this.InitializeComponent();
            this.TaskNavigationViewer.TaskSelectionChangedEvent += (s, e) =>
            {
                var taskViewModel = e.SelectedItem as TaskViewModel;
                if (taskViewModel == null)
                {
                    return;
                }

                var ctrl = mapper.GetControl(taskViewModel.Tag);
                if (ctrl != null)
                {
                    ctrl.DataContext = taskViewModel.DataContext;
                    this.DetailViewContainer.ShowView(ctrl);
                }
            };
        }
    }
}
