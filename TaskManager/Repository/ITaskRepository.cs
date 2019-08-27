using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using TaskManager.Model;
using Utils.Core.Diagnostics;
using Utils.Core.Registration;

namespace TaskManager.Repository
{
    interface ITaskRepository
    {
        Task<IEnumerable<TaskInfo>> GetTasksAsync(IServiceLocator serviceLocator, ILogger logger);
        Task<IEnumerable<Module>> InitializeTaskModules(ContainerBuilder builder, ILogger logger);
    }
}
