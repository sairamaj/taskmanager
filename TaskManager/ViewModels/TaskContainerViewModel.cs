using System.Collections.ObjectModel;
using TaskManager.Views;
using Utils.Core;
using Utils.Core.ViewModels;

namespace TaskManager.ViewModels
{
    class TaskContainerViewModel : CoreViewModel
    {
        public TaskContainerViewModel(ICommandTreeItemViewMapper mapper)
        {
            this.Tasks = new SafeObservableCollection<TaskViewModel>();
            var taskViewModel1 = new TaskViewModel("Dummy1");
            var taskViewModel2 = new TaskViewModel("Dummy2");
            mapper.Add("Dummy1", new DummyTask1View() { DataContext = taskViewModel1 });
            mapper.Add("Dummy2", new DummyTask2View() { DataContext = taskViewModel2 });
            this.Tasks.Add(taskViewModel1);
            this.Tasks.Add(taskViewModel2);
        }

        public ObservableCollection<TaskViewModel> Tasks { get; set; }
    }
}
