using System;
using System.Windows;
using System.Windows.Controls;
using Utils.Core.Views;

namespace JsonExecutorTasks.Views
{
    /// <summary>
    /// Interaction logic for TestFilesContainerView.xaml.
    /// </summary>
    public partial class TestFilesContainerView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFilesContainerView"/> class.
        /// </summary>
        public TestFilesContainerView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Task selection change event in tree.
        /// </summary>
        public event EventHandler<CommandChangeEventArgs> TaskSelectionChangedEvent;

        /// <summary>
        /// Task selected change handler.
        /// </summary>
        /// <param name="sender">
        /// Sender.
        /// </param>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        private void TaskSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.TaskSelectionChangedEvent?.Invoke(this, new CommandChangeEventArgs(e.NewValue));
        }
    }
}
