using System;
using System.Windows;
using System.Windows.Input;

namespace Utils.Core.Command
{
    /// <summary>
    /// This class facilitates associating a key binding in XAML markup to a command
    /// defined in a View Model by exposing a Command dependency property.
    /// The class derives from Freezable to work around a limitation in WPF when data-binding from XAML.
    /// </summary>
    public class CommandReference : Freezable, ICommand
    {
        /// <summary>
        /// Command property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandReference), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Execution changed event.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Gets or sets command.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Determines whether a command can can be executed in current state or not.
        /// </summary>
        /// <param name="parameter">
        /// Command parameter.
        /// </param>
        /// <returns>
        /// True for can execute state.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            if (this.Command != null)
            {
                return this.Command.CanExecute(parameter);
            }

            return false;
        }

        /// <summary>
        /// Executes the associated action.
        /// </summary>
        /// <param name="parameter">
        /// Command parameter.
        /// </param>
        public void Execute(object parameter)
        {
            this.Command.Execute(parameter);
        }

        /// <summary>
        /// Create instance.
        /// </summary>
        /// <returns>
        /// Not implemented.
        /// </returns>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// On command changed handler.
        /// </summary>
        /// <param name="d">
        /// A <see cref="DependencyObject"/> instance.
        /// </param>
        /// <param name="e">
        /// A <see cref="DependencyPropertyChangedEventArgs"/> instance.
        /// </param>
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandReference commandReference = d as CommandReference;
            ICommand oldCommand = e.OldValue as ICommand;
            ICommand newCommand = e.NewValue as ICommand;

            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }
        }
    }
}
