using Tick.Domain.Entities;
using Tick.Domain.Common;
using Tick.Domain.QueryParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tick.Core.Contract.Repository
{
    public interface ITaskRepository : IRepository<Tick.Domain.Entities.Task>
    {
        Task<PagedResponse<List<Tick.Domain.Entities.Task>>> GetTasksAsync(TaskQueryParameters queryParameters);
        Task<List<Tick.Domain.Entities.Task>> GetTasksByUserIdAsync(string userId);
        Task<Tick.Domain.Entities.Task> GetTaskByIdAsync(string taskId);
        Task<Tick.Domain.Entities.Task> AddTaskAsync(Tick.Domain.Entities.Task Task);
        Task<Tick.Domain.Entities.Task> EditTaskAsync(Tick.Domain.Entities.Task Task);
        Task<Tick.Domain.Entities.Task> ToggleCompleteTaskAsync(Tick.Domain.Entities.Task Task);
        Task<string> DeleteTaskAsync(Tick.Domain.Entities.Task Task);
    }
}
