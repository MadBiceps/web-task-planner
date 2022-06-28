

/*var builder = WebApplication.CreateBuilder(args);

// Add env var to configuration
builder.Configuration.AddEnvironmentVariables("TASKLIST_");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration);

var app = builder.Build();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerWeb(app.Services.GetService<IApiVersionDescriptionProvider>());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();*/

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).ConfigureAppConfiguration(config => {
            // Set env var
            config.AddEnvironmentVariables("META_");
        }).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}