using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Utils.Core.Command
{
    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    /// <typeparam name="T">Type of the parameter passed to the delegates.</typeparam>.
    public class DelegateCommand2<T> : ICommand
    {
        /// <summary>
        /// Execute method.
        /// </summary>
        private readonly Action<T> _executeMethod = null;

        /// <summary>
        /// Can execute method.
        /// </summary>
        private readonly Func<T, bool> _canExecuteMethod = null;

        /// <summary>
        /// Automatic re query disabled flag.
        /// </summary>
        private bool _isAutomaticRequeryDisabled = false;

        /// <summary>
        /// Change handlers.
        /// </summary>
        private List<WeakReference> _canExecuteChangedHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand2{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">Action execute method.</param>
        public DelegateCommand2(Action<T> executeMethod)
            : this(executeMethod, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand2{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">Method to be executed.</param>
        /// <param name="canExecuteMethod">Method used to find out whether command can be executed.</param>
        public DelegateCommand2(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand2{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">Action method to execute.</param>
        /// <param name="canExecuteMethod">Method which will be used to find out whether a command can be executed or not.</param>
        /// <param name="isAutomaticRequeryDisabled">Automatic re query disabled flag.</param>
        public DelegateCommand2(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException(nameof(executeMethod));
            }

            this._executeMethod = executeMethod;
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
        /// Gets or sets a value indicating whether property to enable or disable CommandManager's automatic re query on this command.
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get => this._isAutomaticRequeryDisabled;

            set
            {
                if (this._isAutomaticRequeryDisabled != value)
                {
                    if (value)
                    {
                        CommandManager.RemoveHandlersFromRequerySuggested(this._canExecuteChangedHandlers);
                    }
                    else
                    {
                        CommandManager.AddHandlersToRequerySuggested(this._canExecuteChangedHandlers);
                    }

                    this._isAutomaticRequeryDisabled = value;
                }
            }
        }

        /// <summary>
        /// Method to determine if the command can be executed.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>returns whether a command can be executed.</returns>
        public bool CanExecute(T parameter)
        {
            if (this._canExecuteMethod != null)
            {
                return this._canExecuteMethod(parameter);
            }

            return true;
        }

        /// <summary>
        /// Execution of the command.
        /// </summary>
        /// <param name="parameter">Execute parameter.</param>
        public void Execute(T parameter)
        {
            this._executeMethod?.Invoke(parameter);
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
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
            // if T is of value type and the parameter is not
            // set yet, then return false if CanExecute delegate
            // exists, else return true
            if (parameter == null &&
                typeof(T).IsValueType)
            {
                return this._canExecuteMethod == null;
            }

            return this.CanExecute((T)parameter);
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
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
