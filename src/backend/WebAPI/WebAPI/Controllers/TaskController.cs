using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.DTO;
using WebAPI.Services.Task;

namespace WebAPI.Controllers;

[ApiVersion("1.0")]
public class TaskController : ApiController
{
    private readonly ITaskService _taskService;
    private readonly string _baseUrl;
    
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetTasksAsync()
    {
        return Ok(await _taskService.GetTasksAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTaskAsync(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();
        return Ok(task);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateTaskAsync([FromBody] TaskIn taskIn)
    {
        var task = await _taskService.CreateTaskAsync(taskIn.GetMapped());
        return Created($"{_baseUrl}/taks/{task.Id}", task);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTaskAsync(Guid id, [FromBody] TaskIn taskIn)
    {
        return Ok(await _taskService.UpdateTaskAsync(taskIn.GetMapped(id)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTaskAsync(Guid id)
    {
        await _taskService.DeleteTaskAsync(id);
        return Ok();
    }
}