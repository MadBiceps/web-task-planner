using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.DTO;
using WebAPI.Services.List;

namespace WebAPI.Controllers;

[ApiVersion("1.0")]
public class ListController : ApiController
{
    private readonly IListService _listService;
    private readonly string _baseUrl;

    public ListController(IListService listService)
    {
        _listService = listService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetListsAsync()
    {
        return Ok(await _listService.GetListsAsync());
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetListByIdAsync(Guid id)
    {
        var list = await _listService.GetListByIdAsync(id);
        if (list == null)
            return NotFound();
        return Ok(list);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateListAsync([FromBody] ListIn listIn)
    {
        var list = await _listService.CreateListAsync(listIn.GetMapped());
        return Created($"{_baseUrl}/list/{list.Id}", list);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateListAsync(Guid id, [FromBody] ListIn listIn)
    {
        return Ok(await _listService.CreateListAsync(listIn.GetMapped(id)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _listService.DeleteListAsync(id);
        return Ok();
    }

    [HttpGet("{id:guid}/task")]
    public async Task<IActionResult> GetTaskFromListAsync(Guid id)
    {
        var tasks = await _listService.GetTasksFromListAsync(id);
        if (tasks == null)
            return NotFound();
        return Ok(tasks);
    }
    
    [HttpPost("{id:guid}/task/{taskId:guid}")]
    public async Task<IActionResult> AddTaskToListAsync(Guid id, Guid taskId)
    {
        var tasks = await _listService.AddTaskToListAsync(id, taskId);
        if (tasks == null)
            return NotFound();
        return Ok(tasks);
    }
    
    [HttpDelete("{id:guid}/task/{taskId:guid}")]
    public async Task<IActionResult> RemoveTaskFromListAsync(Guid id, Guid taskId)
    {
        var tasks = await _listService.RemoveTaskFromListAsync(id, taskId);
        if (tasks == null)
            return NotFound();
        return Ok(tasks);
    }
}