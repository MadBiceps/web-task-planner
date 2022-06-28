using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPI.Extensions.Swagger;
using WebAPI.Infrastructure;
using WebAPI.Services.List;
using WebAPI.Services.Task;

namespace WebAPI.Extensions;

public static class DataExtension
{
    public static void AddDataProviders(this IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(opt =>
        {
            opt
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        }, ServiceLifetime.Transient);

        // Migrate Database
        var context = services.BuildServiceProvider().GetService<DatabaseContext>();
        
        // Register Services
        services.AddProviders();
    }

    public static void UseSwaggerWeb(this IApplicationBuilder app, IApiVersionDescriptionProvider? provider)
    {
        if (provider == null)
            return;
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // build a swagger endpoint for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }

    public static void ConfigureSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddApiVersioning(options =>
        {
            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;
        });
        serviceCollection.AddVersionedApiExplorer(options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

        serviceCollection.AddMvc(config =>
        {
            config.UseCentralRoutePrefix(
                new RouteAttribute("api/v{version:apiVersion}/[controller]"));
        });

        serviceCollection.Configure<FormOptions>(options => options.ValueCountLimit = int.MaxValue);

        serviceCollection.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        serviceCollection.AddSwaggerGen(options => { options.OperationFilter<SwaggerDefaultValues>(); });
    }

    private static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
    {
        opts.Conventions.Insert(0, new RouteConvention(routeAttribute));
    }

    private static void AddProviders(this IServiceCollection services)
    {
        services.AddScoped<IListService, ListService>();
        services.AddScoped<ITaskService, TaskService>();
    }
}