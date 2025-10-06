using WebAPI.Application;
using WebAPI.Infrastructure;
using WebAPI.API.Extensions;
using WebAPI.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configure settings using Options pattern
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.SectionName));
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(DatabaseSettings.SectionName));

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger based on settings
var apiSettings = builder.Configuration.GetSection(ApiSettings.SectionName).Get<ApiSettings>() ?? new ApiSettings();
if (apiSettings.EnableSwagger)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Title = apiSettings.Title,
            Version = apiSettings.Version,
            Description = apiSettings.Description,
            Contact = new() { Name = apiSettings.ContactName, Email = apiSettings.ContactEmail },
            License = new() { Name = apiSettings.LicenseName }
        });
    });
}

// Add Application and Infrastructure layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add CORS based on settings
if (apiSettings.EnableCors)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(apiSettings.CorsPolicy, policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() && apiSettings.EnableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{apiSettings.Title} {apiSettings.Version}");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app's root
    });
}

app.UseHttpsRedirection();

if (apiSettings.EnableCors)
{
    app.UseCors(apiSettings.CorsPolicy);
}

// Map API endpoints
app.MapProductEndpoints();
app.MapCategoryEndpoints();

app.Run();
