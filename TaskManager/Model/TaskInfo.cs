using System.Windows.Controls;

namespace TaskManager.Model
{
    class TaskInfo
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public UserControl View{ get; set; }
        public object DataContext { get; set; }
    }
}
