using System;
using Demo1Task.Repository;
using DemoStartup;
using Utils.Core.Diagnostics;

namespace Demo1Task.ViewModels
{
    public class RootViewModel 
    {
        public RootViewModel(IDemoRepository demoRepository, ILogger logger)
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
            logger.Debug($"Demo1Task.ViewModels.RootViewModel ctor...");
        }

        public string DemoName { get; set; }
    }
}