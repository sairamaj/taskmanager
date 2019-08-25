using System;
using System.Windows.Controls;
using Utils.Core.ViewModels;

namespace TaskManager.Model
{
    class TaskInfo
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public UserControl View{ get; set; }
        public TaskViewModel TaskViewModel { get; set; }
    }
}
