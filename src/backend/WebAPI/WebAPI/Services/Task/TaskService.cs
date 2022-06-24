using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;

namespace WebAPI.Services.Task;

public class TaskService : ITaskService
{

    private readonly DatabaseContext _databaseContext;
    
    public TaskService(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary>
    /// Gets all tasks from the database
    /// </summary>
    /// <returns>List of task models</returns>
    public async Task<List<Models.Database.Task>> GetTasksAsync()
    {
        return await _databaseContext.Tasks.ToListAsync();
    }
    
    /// <summary>
    /// Gets a task by id
    /// </summary>
    /// <param name="id">Id from the requested list</param>
    /// <returns>Task model from the database or null if the list cannot be found</returns>
    public async Task<Models.Database.Task?> GetTaskByIdAsync(Guid id)
    {
        return await _databaseContext.Tasks.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    /// <summary>
    /// Creates a task in the database
    /// </summary>
    /// <param name="task">Task model with should be added to the database</param>
    /// <returns>Created database model</returns>
    public async Task<Models.Database.Task> CreateTaskAsync(Models.Database.Task task)
    {
        var createdTask = await _databaseContext.AddAsync(task);
        await _databaseContext.SaveChangesAsync();
        return createdTask.Entity;
    }

    /// <summary>
    /// Updates a task in the database
    /// </summary>
    /// <param name="task">Task model with should be updated in the database</param>
    /// <returns>Updated database model</returns>
    public async Task<Models.Database.Task> UpdateTaskAsync(Models.Database.Task task)
    {
        var updatedTask = _databaseContext.Update(task);
        await _databaseContext.SaveChangesAsync();
        return updatedTask.Entity;
    }
    
    /// <summary>
    /// Deletes a task in the database
    /// </summary>
    /// <param name="id">Id of the task</param>
    public async System.Threading.Tasks.Task DeleteTaskAsync(Guid id)
    {
        var entity = await GetTaskByIdAsync(id);
        if (entity == null)
            return;
        _databaseContext.Remove(entity);
        await _databaseContext.SaveChangesAsync();
    }
}