using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Utils.Core.Command
{
    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Execute method.
        /// </summary>
        private readonly Action _executeMethod = null;

        /// <summary>
        /// Can execute method.
        /// </summary>
        private readonly Func<bool> _canExecuteMethod = null;

        /// <summary>
        /// Automatic query disabled flag.
        /// </summary>
        private readonly bool _isAutomaticRequeryDisabled = false;

        /// <summary>
        /// Execute change handlers.
        /// </summary>
        private List<WeakReference> _canExecuteChangedHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        /// <param name="canExecuteMethod">
        /// The can execute method.
        /// </param>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeMethod">
        /// The execute method.
        /// </param>
        /// <param name="canExecuteMethod">
        /// The can execute method.
        /// </param>
        /// <param name="isAutomaticRequeryDisabled">
        /// The is automatic re query disabled.
        /// </param>
        /// <exception cref="ArgumentNullException">If execute method is null.
        /// </exception>
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            this._executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            this._canExecuteMethod = canExecuteMethod;
            this._isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        /// <summary>
        /// ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!this._isAutomaticRequeryDisabled)
                {
                    System.Windows.Input.CommandManager.RequerySuggested += value;
                }

                CommandManager.AddWeakReferenceHandler(ref this._canExecuteChangedHandlers, value, 2);
            }

            remove
            {
                if (!this._isAutomaticRequeryDisabled)
                {
                    System.Windows.Input.CommandManager.RequerySuggested -= value;
                }

                CommandManager.RemoveWeakReferenceHandler(this._canExecuteChangedHandlers, value);
            }
        }

        /// <summary>
        /// Method to determine if the command can be executed.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute()
        {
            if (this._canExecuteMethod != null)
            {
                return this._canExecuteMethod();
            }

            return true;
        }

        /// <summary>
        ///     Execution of the command.
        /// </summary>
        public void Execute()
        {
            this._executeMethod?.Invoke();
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Code copied from some other project.")]
        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        /// <summary>
        /// Can a command be executed or not.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>true if the command can be executed.</returns>
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        /// <summary>
        /// Execute method.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        void ICommand.Execute(object parameter)
        {
            this.Execute();
        }

        /// <summary>
        /// Protected virtual method to raise CanExecuteChanged event.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            CommandManager.CallWeakReferenceHandlers(this._canExecuteChangedHandlers);
        }
    }
}
