using System.Collections.Generic;
using TaskManager.Model;
using TaskManager.Views;
using Utils.Core;

namespace TaskManager.ViewModels
{
    /// <summary>
    /// Task group view model.
    /// </summary>
    internal class TaskGroupViewModel : TaskViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskGroupViewModel"/> class.
        /// </summary>
        /// <param name="mapper">
        /// A <see cref="ICommandTreeItemViewMapper"/> mapper.
        /// </param>
        /// <param name="name">
        /// Group name.
        /// </param>
        /// <param name="tag">
        /// A unique value for the view model.
        /// </param>
        /// <param name="tasks">
        /// A list of tasks.
        /// </param>
        public TaskGroupViewModel(
            ICommandTreeItemViewMapper mapper,
            string name,
            string tag,
            IEnumerable<TaskInfo> tasks)
            : base(null, name, tag)
        {
            this.Mapper = mapper;
            this.Tasks = tasks;
            this.Name = name;
        }

        /// <summary>
        /// Gets command mapper.
        /// </summary>
        public ICommandTreeItemViewMapper Mapper { get; }

        /// <summary>
        /// Gets list of tasks.
        /// </summary>
        public IEnumerable<TaskInfo> Tasks { get; }

        /// <summary>
        /// Load tasks.
        /// </summary>
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
                        DataContext = viewModel,
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
