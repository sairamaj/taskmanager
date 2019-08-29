using System;
using System.Windows.Input;
using Demo1Task.Repository;
using Utils.Core.Command;
using Utils.Core.Diagnostics;

namespace Demo1Task.ViewModels
{
    public class TaskDemoViewModel
    {
        public TaskDemoViewModel(IDemoRepository demoRepository, ILogger logger)
        {
            if (demoRepository == null)
            {
                throw new ArgumentNullException(nameof(demoRepository));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.DemoName = demoRepository.GetDemoName();
            this.LogDebugCommand = new DelegateCommand(() =>
            {
                logger.Debug($"This is debug message:{demoRepository.GetDemoName()}");
            });

            this.LogErrorCommand = new DelegateCommand(() =>
            {
                logger.Error($"This is error message:{demoRepository.GetDemoName()}");
            });

            this.LogInfoCommand = new DelegateCommand(() =>
            {
                logger.Info($"This is info message:{demoRepository.GetDemoName()}");
            });

            logger.Debug($"Demo1Task.ViewModels.RootViewModel ctor...");
        }

        public string DemoName { get; set; }
        public ICommand LogDebugCommand { get; set; }
        public ICommand LogErrorCommand { get; set; }
        public ICommand LogInfoCommand { get; set; }
    }
}