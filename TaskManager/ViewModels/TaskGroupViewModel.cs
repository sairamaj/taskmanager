using System.Collections.Generic;
using TaskManager.Model;
using TaskManager.Views;
using Utils.Core;

namespace TaskManager.ViewModels
{
    class TaskGroupViewModel : TaskViewModel
    {
        public TaskGroupViewModel(
            ICommandTreeItemViewMapper mapper, 
            string name, 
            string tag, 
            IEnumerable<TaskInfo> tasks) : base(null, name, tag)
        {
            Mapper = mapper;
            Tasks = tasks;
            this.Name = name;       // todo: need to debug this why the base is not showing up in UI.
        }

        public ICommandTreeItemViewMapper Mapper { get; }
        public IEnumerable<TaskInfo> Tasks { get; }
        protected override void LoadChildren()
        {
            foreach (var task in this.Tasks)
            {
                if (task.Type == TaskType.TaskWithError)
                {
                    var viewModel = new TaskWithErrorViewModel(task.Name, task.Tag, task.TaskCreationException?.ToString());
                    this.Children.Add(viewModel);
                    this.Mapper.Add(task.Tag, new TaskWithErrorView()
                    {
                        DataContext = viewModel
                    });
                }
                else
                {
                    this.Children.Add(new TaskViewModel(task.Name, task.Tag, task.DataContext));
                    this.Mapper.Add(task.Tag, task.View);
                }
            }
        }
    }
}
