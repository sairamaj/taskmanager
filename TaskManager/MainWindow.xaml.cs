using System.Windows;
using TaskManager.ViewModels;
using Utils.Core;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ICommandTreeItemViewMapper mapper)
        {
            InitializeComponent();
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
