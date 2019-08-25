using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Model;

namespace TaskManager.Repository
{
    interface ITaskRepository
    {
        Task<IEnumerable<TaskInfo>> GetTasksAsync();
    }
}
