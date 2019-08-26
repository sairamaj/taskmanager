using System;
using Demo1Task.Repository;
using Utils.Core.ViewModels;

namespace Demo1Task.ViewModels
{
    public class RootViewModel : TaskViewModel
    {
        public RootViewModel(string name, string tag, IDemoRepository demoRepository) : base(name, tag)
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