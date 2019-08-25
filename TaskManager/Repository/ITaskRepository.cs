using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.Core.ViewModels;

namespace TaskManager.Repository
{
    interface ITaskRepository
    {
        Task<IEnumerable<TaskViewModel>> GetTasksAsync();
    }
}
