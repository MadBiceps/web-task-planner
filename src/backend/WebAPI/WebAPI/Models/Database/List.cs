namespace WebAPI.Models.Database;

public class List
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Task> Tasks { get; set; }
}