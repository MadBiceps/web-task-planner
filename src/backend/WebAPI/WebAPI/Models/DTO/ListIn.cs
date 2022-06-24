using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Database;

namespace WebAPI.Models.DTO;

public class ListIn
{
    [MaxLength(500)]
    public string Name { get; set; }
    public string Description { get; set; }

    public List GetMapped()
    {
        return new List
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Description = Description,
        };
    }
    
    public List GetMapped(Guid id)
    {
        var retVal = GetMapped();
        retVal.Id = id;
        return retVal;
    }
}