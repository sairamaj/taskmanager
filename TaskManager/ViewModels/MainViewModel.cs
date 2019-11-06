using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TaskManager.Repository;
using Utils.Core;
using Utils.Core.Command;
using Utils.Core.Diagnostics;
using Utils.Core.Model;
using Utils.Core.Registration;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    /// <summary>
    /// Main view model.
    /// </summary>
    internal class MainViewModel : CoreViewModel
    {
        /// <summary>
        /// Selected log level.
        /// </summary>
        private string _selectedLogLevel;

        /// <summary>
        /// Filtered log messages collections.
        /// </summary>
        private SafeObservableCollection<LogMessage> _filteredLogMessages = new SafeObservableCollection<LogMessage>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="serviceLocator">
        /// A <see cref="IServiceLocator"/> instance.
        /// </param>
        /// <param name="mapper">
        /// A <see cref="ICommandTreeItemViewMapper"/> mapper.
        /// </param>
        /// <param name="taskRepository">
        /// A <see cref="ITaskRepository"/> repository.
        /// </param>
        public MainViewModel(
            IServiceLocator serviceLocator,
            ICommandTreeItemViewMapper mapper,
            ITaskRepository taskRepository)
        {
            this.TaskContainer = new TaskContainerViewModel(serviceLocator, mapper, taskRepository);
            this.Logger = serviceLocator.Resolve<ILogger>();
            this.ClearCommand = new DelegateCommand(() =>
            {
                this.Logger.Clear();
                this._filteredLogMessages.Clear();
            });
            this.SelectedLogLevel = this.LogLevels.First();
            this.Logger.LogMessages.CollectionChanged += (s, e) =>
            {
                if (this.SelectedLogLevel == "All")
                {
                    return;     // If ALL items then no need to maintain our filtered messages.
                }

                if (e.NewItems == null)
                {
                    this._filteredLogMessages.Clear();
                }
                else
                {
                    e.NewItems.OfType<LogMessage>()
                    .Where(l => l.Level.ToString() == this._selectedLogLevel)
                    .ToList()
                    .ForEach(l => this._filteredLogMessages.Add(l));
                }
            };

            this.Registrations = serviceLocator.GetRegisteredTypes().OrderBy(t => t.InterfaceType.FullName);
        }

        /// <summary>
        /// Gets or sets task container view model.
        /// </summary>
        public TaskContainerViewModel TaskContainer { get; set; }

        /// <summary>
        /// Gets logger instance.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Gets or sets Clear command handler.
        /// </summary>
        public ICommand ClearCommand { get; set; }

        /// <summary>
        /// Gets log messages.
        /// </summary>
        public ObservableCollection<LogMessage> LogMessages
        {
            get
            {
                if (this.SelectedLogLevel == "All")
                {
                    return this.Logger.LogMessages;
                }

                return this._filteredLogMessages;
            }
        }

        /// <summary>
        /// Gets log levels as strin.
        /// </summary>
        public string[] LogLevels => new string[]
        {
            "All",
            LogLevel.Debug.ToString(),
            LogLevel.Error.ToString(),
            LogLevel.Info.ToString(),
        };

        /// <summary>
        /// Gets or sets selected log level.
        /// </summary>
        public string SelectedLogLevel
        {
            get => this._selectedLogLevel;
            set
            {
                this._selectedLogLevel = value;
                if (value == "All")
                {
                    this._filteredLogMessages.Clear();
                }
                else
                {
                    this._filteredLogMessages =
                        new SafeObservableCollection<LogMessage>(
                            this.Logger.LogMessages.Where(l => l.Level.ToString() == value).ToList());
                }

                this.OnPropertyChanged(() => this.LogMessages);
            }
        }

        /// <summary>
        /// Gets registrations.
        /// </summary>
        public IEnumerable<RegistrationInfo> Registrations { get; }

        /// <summary>
        /// Gets http message view model.
        /// </summary>
        public HttpMessagesViewModel HttpMessagesViewModel => Utils.Core.ViewModels.HttpMessagesViewModel.Instance;
    }
}
