using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Services.Task;

namespace WebAPI.Services.List;

public class ListService : IListService
{
    private readonly DatabaseContext _databaseContext;
    private readonly ITaskService _taskService;

    public ListService(DatabaseContext databaseContext, ITaskService taskService)
    {
        _databaseContext = databaseContext;
        _taskService = taskService;
    }

    /// <summary>
    /// Gets all list's from the database
    /// </summary>
    /// <returns>List of list models</returns>
    public async Task<IEnumerable<Models.Database.List>> GetListsAsync()
    {
        return await _databaseContext.Lists.ToListAsync();
    }

    /// <summary>
    /// Gets a list by id
    /// </summary>
    /// <param name="id">Id from the requested list</param>
    /// <returns>List from the database or null if the list cannot be found</returns>
    public async Task<Models.Database.List?> GetListByIdAsync(Guid id)
    {
        return await _databaseContext.Lists.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    /// <summary>
    /// Creates a list in the database
    /// </summary>
    /// <param name="list">List model with should be added to the database</param>
    /// <returns>Created database model</returns>
    public async Task<Models.Database.List> CreateListAsync(Models.Database.List list)
    {
        var createdList = await _databaseContext.AddAsync(list);
        await _databaseContext.SaveChangesAsync();
        return createdList.Entity;
    }

    /// <summary>
    /// Updates a list in the database
    /// </summary>
    /// <param name="list">List model with should be updated in the database</param>
    /// <returns>Updated database model</returns>
    public async Task<Models.Database.List> UpdateListAsync(Models.Database.List list)
    {
        var updatedList = _databaseContext.Update(list);
        await _databaseContext.SaveChangesAsync();
        return updatedList.Entity;
    }

    /// <summary>
    /// Deletes a list in the database
    /// </summary>
    /// <param name="id">Id of the list</param>
    public async System.Threading.Tasks.Task DeleteListAsync(Guid id)
    {
        var entity = await GetListByIdAsync(id);
        if (entity == null)
            return;
        _databaseContext.Remove(entity);
        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Gets all tasks from a list
    /// </summary>
    /// <param name="listId">Id of the list</param>
    /// <returns>Tasks from the list</returns>
    public async Task<IEnumerable<Models.Database.Task>?> GetTasksFromListAsync(Guid listId)
    {
        var list = await _databaseContext.Lists
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id.Equals(listId));
        return list?.Tasks;
    }

    /// <summary>
    /// Add's a task to the list
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="taskId">Task ID</param>
    /// <returns>Updated Task list of the list</returns>
    public async Task<IEnumerable<Models.Database.Task>?> AddTaskToListAsync(Guid listId, Guid taskId)
    {
        var list = await _databaseContext.Lists
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id.Equals(listId));
        if (list == null)
            return null;
        var task = await _taskService.GetTaskByIdAsync(taskId);
        if (task == null)
            return null;
        list.Tasks.Add(task);
        _databaseContext.Update(list);
        await _databaseContext.SaveChangesAsync();
        return list.Tasks;
    }

    /// <summary>
    /// Removes a task from the list
    /// </summary>
    /// <param name="listId">List ID</param>
    /// <param name="taskId">Task ID</param>
    /// <returns>Updated task list from the list</returns>
    public async Task<IEnumerable<Models.Database.Task>?> RemoveTaskFromListAsync(Guid listId, Guid taskId)
    {
        var list = await _databaseContext.Lists
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id.Equals(listId));
        if (list == null)
            return null;
        if(!list.Tasks.Any(x => x.Id.Equals(taskId)))
            return null;
        list.Tasks.RemoveAll(x => x.Id.Equals(taskId));
        _databaseContext.Update(list);
        await _databaseContext.SaveChangesAsync();
        return list.Tasks;
    }
}