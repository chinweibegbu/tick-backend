using Tick.Core.Contract;
using Tick.Core.Contract.Repository;
using Tick.Core.DTO.Request;
using Tick.Core.Implementation;
using Tick.Core.Repository.Base;
using Tick.Domain.Common;
using Tick.Domain.Entities;
using Tick.Domain.Enum;
using Tick.Domain.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading;

namespace Tick.Core.Repository
{
    public class TaskRepository : Repository<Tick.Domain.Entities.Task>, ITaskRepository
    {
        public TaskRepository(IServiceScopeFactory serviceScopeFactory)
            : base(serviceScopeFactory, (context) => context.Set<Tick.Domain.Entities.Task>()){}

        public async Task<PagedResponse<List<Tick.Domain.Entities.Task>>> GetTasksAsync(TaskQueryParameters queryParameters)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                var _tasks = _getDbSet(dbContext);

                var query = _tasks.AsQueryable();

                // Apply filters based on the TaskQueryParameters

                //if (!string.IsNullOrEmpty(queryParameters.Query))
                //{
                //    query = query.Where(x => x.Details.Contains(queryParameters.Query));
                //}

                // Count total records before pagination
                int totalRecords = await query.CountAsync();

                // Apply ordering by creation time, newest first
                // query = query.OrderByDescending(x => x.IsCompleted);

                // Apply pagination
                query = query.Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                             .Take(queryParameters.PageSize);


                var result = await query
                    .AsNoTracking()
                    .ToListAsync();

                return new PagedResponse<List<Tick.Domain.Entities.Task>>(result, queryParameters.PageNumber, queryParameters.PageSize, totalRecords, "Tasks retrieved successfully.");
            }
        }

        public async Task<List<Tick.Domain.Entities.Task>> GetTasksByUserIdAsync(string tickerId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                var _tasks = _getDbSet(dbContext);

                var query = _tasks.AsQueryable();

                // Apply filter based on the specified userId
                query = query.Where(x => x.TickerId == tickerId);

                // Apply grouping by whether or not its completed
                // query = query.GroupBy(x => x.IsCompleted);

                var result = await query
                    .Include(c => c.Ticker)
                    .AsNoTracking()
                .ToListAsync();

                return result;
            }
        }

        public async Task<Tick.Domain.Entities.Task> GetTaskByIdAsync(string taskId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                return await _getDbSet(dbContext)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == taskId);
            }
        }

        public async Task<Tick.Domain.Entities.Task> AddTaskAsync(Tick.Domain.Entities.Task task)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);

                try
                {
                    if (task == null)
                    {
                        // Handle the case where task is null
                        throw new ArgumentNullException(nameof(task), "Task object cannot be null");
                    }

                    await _getDbSet(dbContext).AddAsync(task);
                    await dbContext.SaveChangesAsync();

                    return task;
                }
                catch (DbUpdateException ex)
                {
                    // Handle or log the exception
                    throw new Exception("Error saving changes to the database.", ex);
                }
            }
        }

        public async Task<Tick.Domain.Entities.Task> EditTaskAsync(Tick.Domain.Entities.Task task)
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);
                var existingTask = await _getDbSet(dbContext).FindAsync(task.Id);

                if (existingTask == null)
                {
                    throw new KeyNotFoundException($"Task with ID {task.Id} not found");
                }

                existingTask.Details = task.Details;
                existingTask.IsImportant = task.IsImportant;

                dbContext.Update(existingTask);
                await dbContext.SaveChangesAsync();

                return existingTask;

            }
        }

        public async Task<Tick.Domain.Entities.Task> ToggleCompleteTaskAsync(Tick.Domain.Entities.Task task)
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = GetDatabaseContext(scope);

                // Get existing task
                var existingTask = await _getDbSet(dbContext).FindAsync(task.Id);

                if (existingTask == null)
                {
                    throw new KeyNotFoundException($"Task with ID {existingTask.Id} not found");
                }

                // Negate IsCompleted attribute of existing task
                existingTask.IsCompleted = !existingTask.IsCompleted;


                // Update existing task
                dbContext.Update(existingTask);
                await dbContext.SaveChangesAsync();


                // Return the updated task
                return existingTask;
            }
        }

        public async Task<string> DeleteTaskAsync(string taskId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {

                var dbContext = GetDatabaseContext(scope);

                // Get existing task
                var existingTask = await _getDbSet(dbContext).FindAsync(taskId);

                if (existingTask == null)
                {
                    throw new KeyNotFoundException($"Task with ID {existingTask.Id} not found");
                }

                // Delete existing Task
                _getDbSet(dbContext).Remove(existingTask);
                await dbContext.SaveChangesAsync();

                // Return success
                return $"Task with ID {existingTask.Id} deleted";
            }
        }
    }

}

