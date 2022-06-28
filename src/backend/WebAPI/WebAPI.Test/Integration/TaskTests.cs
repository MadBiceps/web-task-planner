using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using WebAPI.Models.Database;
using WebAPI.Models.DTO;
using WebAPI.Models.Internal;

namespace WebAPI.Test.Integration;

public class TaskTests : IntegrationTest
{
    private readonly List<Task> TaskList = new();

    public TaskTests()
    {
        for (var i = 0; i <= 10; i++)
        {
            TaskList.Add(new Task
            {
                Id = Guid.NewGuid(),
                Name = $"Task No. {i}",
                Description = $"Description of task No. {i}",
                PlannedEndDate = DateTime.Now,
                Status = EStatus.NOT_STARTED
            });
        }
    }

    [SetUp]
    public async System.Threading.Tasks.Task Setup()
    {
        await Database.AddRangeAsync(TaskList);
        await Database.SaveChangesAsync();
    }

    [TearDown]
    public async System.Threading.Tasks.Task TearDown()
    {
        Database.RemoveRange(await Database.Tasks.ToListAsync());
        await Database.SaveChangesAsync();
    }

    [Test]
    public async System.Threading.Tasks.Task CheckGetTaskList()
    {
        var response = await ApiClient.GetAsync("/api/v1/task");

        // Asserts
        Assert.IsTrue(response.IsSuccessStatusCode);
        var responseBody = await response.Content.ReadAsAsync<List<Task>>();
        Assert.AreEqual(responseBody.Count, TaskList.Count);
        foreach (var apiTask in responseBody)
        {
            Assert.Contains(apiTask.Id, TaskList.Select(x => x.Id).ToList());
        }
    }

    [Test]
    public async System.Threading.Tasks.Task CheckGetTask()
    {
        var rnd = new Random();
        foreach (var _ in Enumerable.Range(0, 5))
        {
            var element = TaskList[rnd.Next(TaskList.Count)];

            var response = await ApiClient.GetAsync($"/api/v1/task/{element.Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var responseBody = await response.Content.ReadAsAsync<Task>();
            Assert.AreEqual(responseBody.Id, element.Id);
            Assert.AreEqual(responseBody.Name, element.Name);
            Assert.AreEqual(responseBody.Description, element.Description);
            Assert.AreEqual(responseBody.PlannedEndDate, element.PlannedEndDate);
            Assert.AreEqual(responseBody.Status, element.Status);
        }
    }

    [Test]
    public async System.Threading.Tasks.Task CheckCreateTask()
    {
        var rnd = new Random();
        foreach (var _ in Enumerable.Range(0, 5))
        {
            var taskCreate = new TaskIn
            {
                Name = $"Task {rnd.Next(500)}",
                Description = $"Task description {rnd.Next(500)}",
                Status = EStatus.NOT_STARTED,
                PlannedEndDate = DateTime.Now
            };

            var response = await ApiClient.PostAsync($"/api/v1/task",
                new StringContent(JsonConvert.SerializeObject(taskCreate), System.Text.Encoding.UTF8,
                    "application/json"));
            Assert.IsTrue(response.IsSuccessStatusCode);
            
            var responseBody = await response.Content.ReadAsAsync<Task>();
            Assert.AreEqual(responseBody.Name, taskCreate.Name);
            Assert.AreEqual(responseBody.Description, taskCreate.Description);
            Assert.AreEqual(responseBody.PlannedEndDate, taskCreate.PlannedEndDate);
            Assert.AreEqual(responseBody.Status, taskCreate.Status);
            
            response = await ApiClient.GetAsync($"/api/v1/task/{responseBody.Id}");
            Assert.IsTrue(response.IsSuccessStatusCode);

            var task = await response.Content.ReadAsAsync<Task>();
            Assert.AreEqual(responseBody.Id, task.Id);
        }
    }

    [Test]
    public async System.Threading.Tasks.Task CheckUpdateTask()
    {
        var rnd = new Random();
        var element = TaskList[rnd.Next(TaskList.Count)];

        element.Name = $"Updated Task {rnd.Next(500)}";
        element.Description = $"Updated Task {rnd.Next(500)}";
        element.Status = EStatus.DONE;

        var taskUpdate = new TaskIn
        {
            Name = element.Name,
            Description = element.Description,
            Status = element.Status
        };
        
        
        var response = await ApiClient.PutAsync($"/api/v1/task/{element.Id}",
            new StringContent(JsonConvert.SerializeObject(taskUpdate), System.Text.Encoding.UTF8,
                "application/json"));
        
        var responseBody = await response.Content.ReadAsAsync<Task>();
        Assert.AreEqual(responseBody.Id, element.Id);
        Assert.AreEqual(responseBody.Name, taskUpdate.Name);
        Assert.AreEqual(responseBody.Description, taskUpdate.Description);
        Assert.AreEqual(responseBody.Status, taskUpdate.Status);
        
        response = await ApiClient.GetAsync($"/api/v1/task/{responseBody.Id}");
        Assert.IsTrue(response.IsSuccessStatusCode);

        var task = await response.Content.ReadAsAsync<Task>();
        Assert.AreEqual(responseBody.Id, task.Id);
        Assert.AreEqual(responseBody.Name, task.Name);
        Assert.AreEqual(responseBody.Description, task.Description);
        Assert.AreEqual(responseBody.Status, task.Status);
    }

    [Test]
    public async System.Threading.Tasks.Task CheckDeleteTask()
    {
        var rnd = new Random();
        var element = TaskList[rnd.Next(TaskList.Count)];

        var response = await ApiClient.DeleteAsync($"/api/v1/task/{element.Id}");
        Assert.IsTrue(response.IsSuccessStatusCode);
        
        response = await ApiClient.GetAsync($"/api/v1/task/{element.Id}");
        Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
    }
}