using Tick.Core.Contract;
using Tick.Core.DTO.Request;
using Tick.Core.DTO.Response;
using Tick.Domain.Common;
using Tick.Domain.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tick.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet("getTasks")]
        public async Task<ActionResult<Response<TaskResponse>>> GetTasks([FromQuery] TaskQueryParameters queryParameters)
        {
            return Ok(await _taskService.GetTasksAsync(queryParameters, HttpContext.RequestAborted));
        }

        [HttpGet("getTasksByUserId/{userId}")]
        public async Task<ActionResult<Response<TaskResponse>>> GetTasksByUserId(string userId)
        {
            return Ok(await _taskService.GetTasksByUserIdAsync(userId, HttpContext.RequestAborted));
        }

        [HttpGet("getTaskById/{taskId}")]
        public async Task<ActionResult<Response<TaskResponse>>> GetTaskById(string taskId)
        {
            return Ok(await _taskService.GetTaskById(taskId, HttpContext.RequestAborted));
        }

        [HttpPost("addTask")]
        public async Task<ActionResult<Response<string>>> AddTask(AddTaskRequest request)
        {
            return Ok(await _taskService.AddTaskAsync(request, HttpContext.RequestAborted));
        }

        [HttpPost("editTask/{taskId}")]
        public async Task<ActionResult<Response<string>>> EditTask([FromBody] EditTaskRequest request, string taskId)
        {
            return Ok(await _taskService.EditTaskAsync(request, taskId, HttpContext.RequestAborted));
        }

        [HttpPost("toggleCompleteTask/{taskId}")]
        public async Task<ActionResult<Response<string>>> ToggleCompleteask([FromBody] EditTaskRequest request, string taskId)
        {
            return Ok(await _taskService.ToggleCompleteTaskAsync(taskId, HttpContext.RequestAborted));
        }

        [HttpPost("deleteTask/{taskId}")]
        public async Task<ActionResult<Response<string>>> DeleteTask(string taskId)
        {
            return Ok(await _taskService.DeleteTaskAsync(taskId, HttpContext.RequestAborted));
        }
    }
}