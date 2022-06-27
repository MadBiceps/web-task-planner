using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WebAPI.Models.Database;
using WebAPI.Models.Internal;
using List = WebAPI.Models.Database.List;

namespace WebAPI.Test.Integration;

public class ListTest : IntegrationTest
{
    private readonly List<List> Lists = new();

    public ListTest()
    {
        for (var i = 0; i <= 10; i++)
        {
            Lists.Add(new List
            {
                Id = Guid.NewGuid(),
                Name = $"List No. {i}",
                Description = $"Description of list No. {i}",
                Tasks = new List<Task>()
            });
        }

        foreach (var list in Lists)
        {
            for (var i = 0; i <= 10; i++)
            {
                list.Tasks.Add(new Task
                {
                    Id = Guid.NewGuid(),
                    Name = $"Task No. {i}",
                    Description = $"Description of task No. {i}",
                    PlannedEndDate = DateTime.Now,
                    Status = EStatus.NOT_STARTED
                });
            }
        }
    }

    [SetUp]
    public async System.Threading.Tasks.Task Setup()
    {
        await Database.AddRangeAsync(Lists);
        await Database.SaveChangesAsync();
    }

    [TearDown]
    public async System.Threading.Tasks.Task TearDown()
    {
        Database.RemoveRange(await Database.Lists.ToListAsync());
        await Database.SaveChangesAsync();
    }
    
    

}