namespace WebAPI.Services.List;

public interface IListService
{
    /// <summary>
    /// Gets all list's from the database
    /// </summary>
    /// <returns>List of list models</returns>
    public Task<IEnumerable<Models.Database.List>> GetListsAsync();

    /// <summary>
    /// Gets a list by id
    /// </summary>
    /// <param name="id">Id from the requested list</param>
    /// <returns>List from the database or null if the list cannot be found</returns>
    public Task<Models.Database.List?> GetListByIdAsync(Guid id);

    /// <summary>
    /// Creates a list in the database
    /// </summary>
    /// <param name="list">List model with should be added to the database</param>
    /// <returns>Created database model</returns>
    public Task<Models.Database.List> CreateListAsync(Models.Database.List list);

    /// <summary>
    /// Updates a list in the database
    /// </summary>
    /// <param name="list">List model with should be updated in the database</param>
    /// <returns>Updated database model</returns>
    public Task<Models.Database.List> UpdateListAsync(Models.Database.List list);

    /// <summary>
    /// Deletes a list in the database
    /// </summary>
    /// <param name="id">Id of the list</param>
    public System.Threading.Tasks.Task DeleteListAsync(Guid id);

    /// <summary>
    /// Gets all tasks from a list
    /// </summary>
    /// <param name="listId">Id of the list</param>
    /// <returns>Tasks from the list</returns>
    public Task<IEnumerable<Models.Database.Task>?> GetTasksFromListAsync(Guid listId);

    /// <summary>
    /// Add's a task to the list
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="taskId">Task ID</param>
    /// <returns>Updated Task list of the list</returns>
    public Task<IEnumerable<Models.Database.Task>?> AddTaskToListAsync(Guid listId, Guid taskId);

    /// <summary>
    /// Removes a task from the list
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="taskId">Task ID</param>
    /// <returns>Updated task list from the list</returns>
    public Task<IEnumerable<Models.Database.Task>?> RemoveTaskFromListAsync(Guid listId, Guid taskId);
}