using WebAPI.Models.Internal;
using Task = WebAPI.Models.Database.Task;

namespace WebAPI.Models.DTO;

public class TaskIn
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EStatus Status { get; set; }
    
    
    public Task GetMapped()
    {
        return new Task
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Description = Description,
            PlannedEndDate = PlannedEndDate,
            Status = Status,
            StartDate = StartDate,
            EndDate = EndDate
        };
    }
    
    public Task GetMapped(Guid id)
    {
        var retVal = GetMapped();
        retVal.Id = id;
        return retVal;
    }
}