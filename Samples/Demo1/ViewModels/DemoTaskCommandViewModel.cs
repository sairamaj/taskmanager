using Utils.Core.ViewModels;

namespace Demo1.ViewModels
{
    class DemoTaskCommandViewModel : CommandTreeViewModel
    {
        public DemoTaskCommandViewModel(string name, string tag) : base(null, name, tag)
        {
        }
    }
}
