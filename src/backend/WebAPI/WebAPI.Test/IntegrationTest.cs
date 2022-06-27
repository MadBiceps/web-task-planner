using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebAPI.Infrastructure;

namespace WebAPI.Test;

public class IntegrationTest
{
    protected readonly HttpClient ApiClient;
    protected readonly DatabaseContext Database;
    
    protected IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Name has to be unique per instance because if not tests cant run in parallel
                    var databaseName = Guid.NewGuid().ToString();
                    // Setup database
                    services.RemoveAll(typeof(DatabaseContext));
                    services.RemoveAll(typeof(IDbContextFactory<DatabaseContext>));
                    services.RemoveAll(typeof(DbContextOptions<DatabaseContext>));
                    services.AddDbContext<DatabaseContext>(opt => opt
                        .UseInMemoryDatabase(databaseName)
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging(), ServiceLifetime.Transient);
                    services.AddDbContextFactory<DatabaseContext>(opt => opt
                        .UseInMemoryDatabase(databaseName)
                        .EnableDetailedErrors()
                        .EnableSensitiveDataLogging(), ServiceLifetime.Transient);
                });
            });

        ApiClient = appFactory.CreateClient();
        var serviceScope = appFactory.Services.CreateScope();
        Database = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
    }
}