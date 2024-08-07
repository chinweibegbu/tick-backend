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

        public async Task<List<TaskResponse>> GetTasksByUserIdAsync(string tickerId, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByUserIdAsync(tickerId);

            // Map the list of tasks to a list of TaskResponse objects using the injected IMapper
            var taskResponses = _mapper.Map<List<Tick.Domain.Entities.Task>, List<TaskResponse>>(tasks);

            return taskResponses;
        }

        public async Task<Response<TaskResponse>> GetTaskById(string taskId, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);

            // Handle the case where the task is not found
            if (task == null)
            {
                return null;
            }

            // Map the retrieved task to a TaskResponse object using the injected IMapper
            var response = _mapper.Map<TaskResponse>(task);
            return new Response<TaskResponse>(response);
        }

        public async Task<Response<TaskResponse>> AddTaskAsync(AddTaskRequest taskRequest, CancellationToken cancellationToken)
        {
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            var taskEntity = _mapper.Map<Tick.Domain.Entities.Task>(taskRequest);
            taskEntity.TickerId = userId;
            taskEntity.IsCompleted = false;
            taskEntity.CreatedAt = DateTime.Now;

            // Call repository method to add task
            var addedTask = await _taskRepository.AddTaskAsync(taskEntity);

            var response = _mapper.Map<TaskResponse>(addedTask);
            return new Response<TaskResponse>(response, message: $"Task Added Successfully.");
        }

        public async Task<Response<TaskResponse>> EditTaskAsync(EditTaskRequest taskRequest, string taskId, CancellationToken cancellationToken)
        {
            // Confirm that a user is logged in
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ApiException("Please Login.");
            }

            // Retrieve the existing task by ID
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
            if (existingTask == null)
            {
                throw new ApiException($"Task with ID {taskId} not found");
            }

            // Ensure the task belongs to the current user
            if (existingTask.TickerId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }
            // Map the incoming update request onto the existing TASK entity
            _mapper.Map(taskRequest, existingTask);

            var updatedTask = await _taskRepository.EditTaskAsync(existingTask);

            var response = _mapper.Map<TaskResponse>(updatedTask);
            return new Response<TaskResponse>(response, message: "Task Updated Successfully");
        }

        public async Task<Response<TaskResponse>> ToggleCompleteTaskAsync(string taskId, CancellationToken cancellationToken)
        {
            // Confirm that a user is logged in
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ApiException("Please Login.");
            }

            // Retrieve the existing task by ID
            var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
            if (existingTask == null)
            {
                throw new ApiException($"Task with ID {taskId} not found");
            }

            // Ensure the task belongs to the current user
            if (existingTask.TickerId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this task.");
            }

            var updatedTask = await _taskRepository.ToggleCompleteTaskAsync(existingTask);

            var response = _mapper.Map<TaskResponse>(updatedTask);
            return new Response<TaskResponse>(response, message: "Task Updated Successfully");
        }

        public async Task<Response<string>> DeleteTaskAsync(string taskId, CancellationToken cancellationToken)
        {
            // Confirm that a user is logged in
            string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ApiException("Please Login.");
            }
            
            // Delete Task
            try
            {
                var result = await _taskRepository.DeleteTaskAsync(taskId);
            } catch (Exception ex) {
                throw new ApiException(ex.Message);
            }

            return new Response<string>(taskId, message: $"Successfully deleted the Task.");
        }
    }
}
