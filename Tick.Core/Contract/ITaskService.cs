using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Domain.Common;
using Tick.Domain.QueryParameters;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tick.Core.Repository;

namespace Tick.Core.Contract
{
    public interface ITaskService
    {
        Task<PagedResponse<List<TaskResponse>>> GetTasksAsync(TaskQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<List<TaskResponse>> GetTasksByUserIdAsync(CancellationToken cancellationToken);
        Task<Response<TaskResponse>> GetTaskById(string taskId, CancellationToken cancellationToken);
        Task<Response<TaskResponse>> AddTaskAsync(AddTaskRequest request, CancellationToken cancellationToken);
        Task<Response<TaskResponse>> EditTaskAsync(EditTaskRequest request, string taskId, CancellationToken cancellationToken);
        Task<Response<TaskResponse>> ToggleCompleteTaskAsync(string taskId, CancellationToken cancellationToken);
        Task<Response<string>> DeleteTaskAsync(string taskId, CancellationToken cancellationToken);
    }
}
