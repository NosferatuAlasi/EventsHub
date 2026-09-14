using System.Reflection;
using EventsHub.API.Controllers;
using EventsHub.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// EventsController takes AppDbContext as a constructor dependency; register it
// so AddControllersAsServices() can resolve the controller. The doc/schema
// generation never queries the database, so a throwaway Sqlite in-memory
// connection is enough.
services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=:memory:");
});

services
    .AddOpenApiDocument(document =>
    {
        document.DocumentName = "EventsHub";
        document.Title = "EventsHubV1"; // Official interface name. No spaces. PascalCase.
        document.Version = "1.0.0";
        document.DefaultResponseReferenceTypeNullHandling =
            NJsonSchema.Generation.ReferenceTypeNullHandling.NotNull;
    });

var pluginAssembly = Assembly.GetAssembly(typeof(WeatherForecastController));
services.AddMvc()
    .AddApplicationPart(pluginAssembly!)
    .AddControllersAsServices()
    .AddNewtonsoftJson(options =>
    {
        // Match the API's camelCase JSON output.
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

var app = builder.Build();
app.UseOpenApi();
app.UseSwaggerUi();
app.MapControllers();
app.Run();
