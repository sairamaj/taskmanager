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
    class MainViewModel : CoreViewModel
    {
        private string _selectedLogLevel;
        private SafeObservableCollection<LogMessage> _filteredLogMessages = new SafeObservableCollection<LogMessage>();
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
            this.SelectedLogLevel = LogLevels.First();
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

            this.Registrations = serviceLocator.GetRegisteredTypes().OrderBy(t=>t.InterfaceType.FullName);
        }

        public TaskContainerViewModel TaskContainer { get; set; }
        public ILogger Logger { get; private set; }
        public ICommand ClearCommand { get; set; }

        public ObservableCollection<LogMessage> LogMessages
        {
            get
            {
                if (this.SelectedLogLevel == "All")
                {
                    return this.Logger.LogMessages;
                }

                return _filteredLogMessages;
            }
        }

        public string[] LogLevels => new string[]
        {
            "All",
            LogLevel.Debug.ToString(),
            LogLevel.Error.ToString(),
            LogLevel.Info.ToString(),
        };

        public string SelectedLogLevel
        {
            get => _selectedLogLevel;
            set
            {
                _selectedLogLevel = value;
                if (value == "All")
                {
                    _filteredLogMessages.Clear();
                }
                else
                {
                    _filteredLogMessages =
                        new SafeObservableCollection<LogMessage>(
                            this.Logger.LogMessages.Where(l => l.Level.ToString() == value).ToList());
                }

                OnPropertyChanged(() => LogMessages);
            }
        }

        public IEnumerable<RegistrationInfo> Registrations { get; private set; }
    }
}
