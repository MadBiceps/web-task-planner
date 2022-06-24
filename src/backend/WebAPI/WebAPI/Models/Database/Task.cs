using WebAPI.Models.Internal;

namespace WebAPI.Models.Database;

public class Task
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EStatus Status { get; set; }
    public Category? Category { get; set; }
}