using System.Windows;
using Utils.Core;
using Utils.Core.ViewModels;

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
                var cmdViewModel = e.SelectedItem as CommandTreeViewModel;
                if (cmdViewModel == null)
                {
                    return;
                }

                var ctrl = mapper.GetControl(cmdViewModel.Tag);
                if (ctrl != null)
                {
                    ctrl.DataContext = cmdViewModel;
                    this.DetailViewContainer.ShowView(ctrl);
                }

            };
        }
    }
}
