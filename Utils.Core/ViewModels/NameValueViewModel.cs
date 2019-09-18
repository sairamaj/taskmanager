using System;

namespace Utils.Core.ViewModels
{
    public class NameValueViewModel : CoreViewModel
    {
        public NameValueViewModel(string name, string value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
