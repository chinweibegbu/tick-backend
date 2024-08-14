using Tick.Core.Contract;
using Tick.Core.Contract.Repository;
using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Core.Exceptions;
using Tick.Domain.Common;
using Tick.Domain.Entities;
using Tick.Domain.QueryParameters;
using Tick.Domain.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;

namespace Tick.Core.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskService(
            ITaskRepository taskRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResponse<List<TaskResponse>>> GetTasksAsync(TaskQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var pagedTasks = await _taskRepository.GetTasksAsync(queryParameters);

            // Map the list of tasks to a list of TaskResponse objects using the injected IMapper
            var taskResponses = _mapper.Map<List<Tick.Domain.Entities.Task>, List<TaskResponse>>(pagedTasks.Data);

            return new PagedResponse<List<TaskResponse>>(taskResponses, pagedTasks.PageMeta.PageNumber, pagedTasks.PageMeta.PageSize, pagedTasks.PageMeta.TotalRecords, "Tasks Retrieved Successfully");
        }

        public async Task<Response<List<TaskResponse>>> GetTasksByUserIdAsync(CancellationToken cancellationToken)
        {
            // Get user from HTTPContextAccessor
            string tickerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            // Get tasks by user
            var tasks = await _taskRepository.GetTasksByUserIdAsync(tickerId);

            // Map the list of tasks to a list of TaskResponse objects using the injected IMapper
            var taskResponses = _mapper.Map<List<Tick.Domain.Entities.Task>, List<TaskResponse>>(tasks);

            return new Response<List<TaskResponse>>(taskResponses, "Fetched tasks by user ID");
        }

        public async Task<Response<TaskResponse>> GetTaskById(string taskId, CancellationToken cancellationToken)
        {
            // Confirm that a user is logged in
            string tickerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(tickerId))
            {
                throw new ApiException("Please Login.", httpStatusCode:HttpStatusCode.Unauthorized);
            }

            // Retrieve the existing task by ID
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
            if (existingTask == null)
            {
                throw new ApiException($"Task with ID {taskId} not found", httpStatusCode:HttpStatusCode.NotFound);
            }

            // Ensure the exsiting task belongs to the current user
            if (existingTask.TickerId != tickerId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }

            // Map the retrieved task to a TaskResponse object using the injected IMapper
            var response = _mapper.Map<TaskResponse>(existingTask);
            return new Response<TaskResponse>(response);
        }

        public async Task<Response<TaskResponse>> AddTaskAsync(AddTaskRequest taskRequest, CancellationToken cancellationToken)
        {
            string tickerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            var taskEntity = _mapper.Map<Tick.Domain.Entities.Task>(taskRequest);
            taskEntity.TickerId = tickerId;
            taskEntity.IsCompleted = false;
            taskEntity.CreatedAt = DateTime.Now;

            // Add created task to DB using repository call
            var addedTask = await _taskRepository.AddTaskAsync(taskEntity);

            var response = _mapper.Map<TaskResponse>(addedTask);
            return new Response<TaskResponse>(response, message: $"Task Added Successfully.");
        }

        public async Task<Response<TaskResponse>> EditTaskAsync(EditTaskRequest taskRequest, string taskId, CancellationToken cancellationToken)
        {
            // Confirm that a user is logged in
            string tickerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(tickerId))
            {
                throw new ApiException("Please Login.", httpStatusCode:HttpStatusCode.Unauthorized);
            }

            // Retrieve the existing task by ID
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
            if (existingTask == null)
            {
                throw new ApiException($"Task with ID {taskId} not found", httpStatusCode:HttpStatusCode.NotFound);
            }

            // Ensure the exsiting task belongs to the current user
            if (existingTask.TickerId != tickerId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }

            // Update task for specified fields
            existingTask.Details = string.IsNullOrEmpty(taskRequest.Details) ? existingTask.Details : taskRequest.Details;
            existingTask.IsImportant = (taskRequest.IsImportant == null) ? existingTask.IsImportant : taskRequest.IsImportant.Value;

            // Update existing task in DB with repository call
            var updatedTask = await _taskRepository.EditTaskAsync(existingTask);

            var response = _mapper.Map<TaskResponse>(updatedTask);
            return new Response<TaskResponse>(response, message: "Task Updated Successfully");
        }

        public async Task<Response<TaskResponse>> ToggleCompleteTaskAsync(string taskId, CancellationToken cancellationToken)
        {
            // Confirm that a user is logged in
            string tickerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(tickerId))
            {
                throw new ApiException("Please Login.", httpStatusCode:HttpStatusCode.Unauthorized);
            }

            // Retrieve the existing task by ID
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
            if (existingTask == null)
            {
                throw new ApiException($"Task with ID {taskId} not found", httpStatusCode:HttpStatusCode.NotFound);
            }

            // Ensure the existing task belongs to the current user
            if (existingTask.TickerId != tickerId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }

            // Toggle complete property of existing task
            existingTask.IsCompleted = !existingTask.IsCompleted;

            // Update existing task in DB with repository call
            var updatedTask = await _taskRepository.EditTaskAsync(existingTask);

            var response = _mapper.Map<TaskResponse>(existingTask);
            return new Response<TaskResponse>(response, message: "Task Updated Successfully");
        }

        public async Task<Response<string>> DeleteTaskAsync(string taskId, CancellationToken cancellationToken)
        {
            // Confirm that a user is logged in
            string tickerId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(tickerId))
            {
                throw new ApiException("Please Login.", httpStatusCode:HttpStatusCode.Unauthorized);
            }

            // Retrieve the existing task by ID
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
            if (existingTask == null)
            {
                throw new ApiException($"Task with ID {taskId} not found", httpStatusCode:HttpStatusCode.NotFound);
            }

            // Ensure the task belongs to the current user
            if (existingTask.TickerId != tickerId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }

            // Delete existing task from DB with repository call
            await _taskRepository.DeleteTaskAsync(existingTask);

            return new Response<string>("Task Deleted Successfully");;
        }
    }
}
