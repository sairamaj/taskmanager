using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using TaskManager.Model;
using Utils.Core.Diagnostics;
using Utils.Core.Registration;

namespace TaskManager.Repository
{
    /// <summary>
    /// Repository for getting task information.
    /// </summary>
    internal interface ITaskRepository
    {
        /// <summary>
        /// Gets all tasks information.
        /// </summary>
        /// <param name="serviceLocator">
        /// A <see cref="IServiceLocator"/> instance used for resolving dependencies.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger"/> instance.
        /// </param>
        /// <returns>
        /// A <see cref="IEnumerable{T}"/> of tasks info.
        /// </returns>
        Task<IEnumerable<TaskInfo>> GetTasksAsync(IServiceLocator serviceLocator, ILogger logger);

        /// <summary>
        /// Initializes task modules.
        /// </summary>
        /// <param name="builder">
        /// A <see cref="ContainerBuilder"/> instance.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger"/> instance.
        /// </param>
        /// <returns>
        /// A task object.
        /// </returns>
        Task InitializeTaskModulesAsync(ContainerBuilder builder, ILogger logger);
    }
}
