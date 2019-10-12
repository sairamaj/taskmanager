using System;
using System.Windows;
using System.Windows.Controls;
using Utils.Core.Views;

namespace JsonExecutorTasks.Views
{
    /// <summary>
    /// Interaction logic for TestFilesContainerView.xaml
    /// </summary>
    public partial class TestFilesContainerView : UserControl
    {
        public TestFilesContainerView()
        {
            InitializeComponent();
        }

        public event EventHandler<CommandChangeEventArgs> TaskSelectionChangedEvent;

        private void TaskSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TaskSelectionChangedEvent?.Invoke(this, new CommandChangeEventArgs(e.NewValue));
        }

    }
}
