namespace WebAPI.Services.Task;

public interface ITaskService
{
    /// <summary>
    /// Gets all tasks from the database
    /// </summary>
    /// <returns>List of task models</returns>
    public Task<List<Models.Database.Task>> GetTasksAsync();

    /// <summary>
    /// Gets a task by id
    /// </summary>
    /// <param name="id">Id from the requested list</param>
    /// <returns>Task model from the database or null if the list cannot be found</returns>
    public Task<Models.Database.Task?> GetTaskByIdAsync(Guid id);

    /// <summary>
    /// Creates a task in the database
    /// </summary>
    /// <param name="task">Task model with should be added to the database</param>
    /// <returns>Created database model</returns>
    public Task<Models.Database.Task> CreateTaskAsync(Models.Database.Task task);

    /// <summary>
    /// Updates a task in the database
    /// </summary>
    /// <param name="task">Task model with should be updated in the database</param>
    /// <returns>Updated database model</returns>
    public Task<Models.Database.Task> UpdateTaskAsync(Models.Database.Task task);

    /// <summary>
    /// Deletes a task in the database
    /// </summary>
    /// <param name="id">Id of the task</param>
    public System.Threading.Tasks.Task DeleteTaskAsync(Guid id);
}