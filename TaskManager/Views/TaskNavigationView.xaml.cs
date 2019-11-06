using System;
using System.Windows;
using Utils.Core.Views;

namespace TaskManager.Views
{
    /// <summary>
    /// Interaction logic for TaskNavigationView.xaml.
    /// </summary>
    public partial class TaskNavigationView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNavigationView"/> class.
        /// </summary>
        public TaskNavigationView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Task selection change event.
        /// </summary>
        public event EventHandler<CommandChangeEventArgs> TaskSelectionChangedEvent;

        /// <summary>
        /// Command selected item changed method.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void TaskSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.TaskSelectionChangedEvent?.Invoke(this, new CommandChangeEventArgs(e.NewValue));
        }
    }
}
