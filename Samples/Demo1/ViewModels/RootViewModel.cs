using System;
using Demo1Task.Repository;

namespace Demo1Task.ViewModels
{
    public class RootViewModel 
    {
        public RootViewModel(IDemoRepository demoRepository)
        {
            if (demoRepository == null)
            {
                throw new ArgumentNullException(nameof(demoRepository));
            }

            this.DemoName = demoRepository.GetDemoName();
        }

        public string DemoName { get; set; }
    }
}