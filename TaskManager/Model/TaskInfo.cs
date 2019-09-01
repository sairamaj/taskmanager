using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TaskManager.Model
{
    class TaskInfo
    {
        public TaskType Type { get; set; }
        public string Name { get; set; }
        public Exception TaskCreationException { get; set; }
        public string Tag { get; set; }
        public UserControl View{ get; set; }
        public object DataContext { get; set; }
        public IEnumerable<TaskInfo> Tasks { get; set; }
    }
}
