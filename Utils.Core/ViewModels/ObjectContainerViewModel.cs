using System.Collections.Generic;

namespace Utils.Core.ViewModels
{
    public class ObjectContainerViewModel : TreeViewItemViewModel
    {
        private readonly object _obj;

        public ObjectContainerViewModel(TreeViewItemViewModel parent, object obj, string name)
            : base(parent, name, true)
        {
            _obj = obj;
        }

        public List<ObjectTreeViewModel> Objects => new List<ObjectTreeViewModel>() { new ObjectTreeViewModel(this, Name, _obj, ObjectTreeViewModel.InfoType.Properties) };
    }
}
