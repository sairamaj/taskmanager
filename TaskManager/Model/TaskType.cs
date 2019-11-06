namespace TaskManager.Model
{
    /// <summary>
    /// Task type information.
    /// </summary>
    internal enum TaskType
    {
        /// <summary>
        /// Regular task.
        /// </summary>
        Task,

        /// <summary>
        /// Task with error in loading.
        /// </summary>
        TaskWithError,

        /// <summary>
        /// Task group.
        /// </summary>
        TaskGroup,
    }
}
