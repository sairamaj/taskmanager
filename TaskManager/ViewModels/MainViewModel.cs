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
            });
            this.SelectedLogLevel = LogLevels.First();
        }

        public TaskContainerViewModel TaskContainer { get; set; }
        public ILogger Logger { get; private set; }
        public ICommand ClearCommand { get; set; }

        public string[] LogLevels => new string[]
        {
            "All",
            LogLevel.Debug.ToString(),
            LogLevel.Error.ToString(),
            LogLevel.Info.ToString(),
        };

        public string SelectedLogLevel { get; set; }
    }
}
