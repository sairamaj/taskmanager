using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace TaskManager.Model
{
    /// <summary>
    /// Task information.
    /// </summary>
    internal class TaskInfo
    {
        /// <summary>
        /// Gets or sets task type.
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        /// Gets or sets task name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets task creation exception.
        /// </summary>
        public Exception TaskCreationException { get; set; }

        /// <summary>
        /// Gets or sets tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets user view associated with this task.
        /// </summary>
        public UserControl View { get; set; }

        /// <summary>
        /// Gets or sets data context associated with this task.
        /// </summary>
        public object DataContext { get; set; }

        /// <summary>
        /// Gets or sets all tasks associated with this task container.
        /// </summary>
        public IEnumerable<TaskInfo> Tasks { get; set; }
    }
}
