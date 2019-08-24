using System;
using System.Windows;
using System.Windows.Controls;
using Utils.Core.Views;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for TaskNavigationView.xaml
    /// </summary>
    public partial class TaskNavigationView : UserControl
    {
        public TaskNavigationView()
        {
            InitializeComponent();
        }

        public event EventHandler<CommandChangeEventArgs> TaskSelectionChangedEvent;

        /// <summary>
        /// Command selected item changed method.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void TaskSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TaskSelectionChangedEvent?.Invoke(this, new CommandChangeEventArgs(e.NewValue));
        }
    }
}
