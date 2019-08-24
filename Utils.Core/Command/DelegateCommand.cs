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
        #region Constructors

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
        /// <exception cref="ArgumentNullException">If execute method is null
        /// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Requery")]
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            _executeMethod = executeMethod ?? throw new ArgumentNullException(nameof(executeMethod));
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to determine if the command can be executed
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute()
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod();
            }

            return true;
        }

        /// <summary>
        ///     Execution of the command
        /// </summary>
        public void Execute()
        {
            _executeMethod?.Invoke();
        }

        /// <summary>
        /// Raises the CanExecuteChanged event
        /// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Justification = "Code copied from some other project.")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// Protected virtual method to raise CanExecuteChanged event
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            CommandManager.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
        }

        #endregion

        #region ICommand Members

        /// <summary>
        ///     ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    System.Windows.Input.CommandManager.RequerySuggested += value;
                }

                CommandManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }

            remove
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    System.Windows.Input.CommandManager.RequerySuggested -= value;
                }

                CommandManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
        }

        /// <summary>
        /// Can a command be executed or not.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>true if the command can be executed.</returns>
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }

        /// <summary>
        /// Execute method.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        void ICommand.Execute(object parameter)
        {
            Execute();
        }

        #endregion

        #region Data

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
        private bool _isAutomaticRequeryDisabled = false;

        /// <summary>
        /// Execute change handlers.
        /// </summary>
        private List<WeakReference> _canExecuteChangedHandlers;

        #endregion
    }

    /// <summary>
    /// This class allows delegating the commanding logic to methods passed as parameters,
    /// and enables a View to bind commands to objects that are not part of the element tree.
    /// </summary>
    /// <typeparam name="T">Type of the parameter passed to the delegates</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">Action execute method.</param>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">Method to be executed.</param>
        /// <param name="canExecuteMethod">Method used to find out whether command can be executed.</param>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand{T}"/> class.
        /// </summary>
        /// <param name="executeMethod">Action method to execute.</param>
        /// <param name="canExecuteMethod">Method which will be used to find out whether a command can be executed or not.</param>
        /// <param name="isAutomaticRequeryDisabled">Automatic re query disabled flag.</param>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to determine if the command can be executed.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>returns whether a command can be executed.</returns>
        public bool CanExecute(T parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }

            return true;
        }

        /// <summary>
        /// Execution of the command
        /// </summary>
        /// <param name="parameter">Execute parameter.</param>
        public void Execute(T parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }
        }

        /// <summary>
        /// Raises the CanExecuteChanged event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// Protected virtual method to raise CanExecuteChanged event
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            CommandManager.CallWeakReferenceHandlers(_canExecuteChangedHandlers);
        }

        /// <summary>
        /// Gets or sets a value indicating whether property to enable or disable CommandManager's automatic re query on this command
        /// </summary>
        public bool IsAutomaticRequeryDisabled
        {
            get
            {
                return _isAutomaticRequeryDisabled;
            }

            set
            {
                if (_isAutomaticRequeryDisabled != value)
                {
                    if (value)
                    {
                        CommandManager.RemoveHandlersFromRequerySuggested(_canExecuteChangedHandlers);
                    }
                    else
                    {
                        CommandManager.AddHandlersToRequerySuggested(_canExecuteChangedHandlers);
                    }

                    _isAutomaticRequeryDisabled = value;
                }
            }
        }

        #endregion

        #region ICommand Members

        /// <summary>
        ///     ICommand.CanExecuteChanged implementation
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    System.Windows.Input.CommandManager.RequerySuggested += value;
                }

                CommandManager.AddWeakReferenceHandler(ref _canExecuteChangedHandlers, value, 2);
            }

            remove
            {
                if (!_isAutomaticRequeryDisabled)
                {
                    System.Windows.Input.CommandManager.RequerySuggested -= value;
                }

                CommandManager.RemoveWeakReferenceHandler(_canExecuteChangedHandlers, value);
            }
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
                return (_canExecuteMethod == null);
            }

            return CanExecute((T)parameter);
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

        #endregion

        #region Data

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

        #endregion
    }
}
