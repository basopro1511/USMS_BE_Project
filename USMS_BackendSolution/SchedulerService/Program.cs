using SchedulerDataAccess.Core;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI with detailed information
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SchedulerService API",
        Version = "v1",
        Description = "API for managing scheduling in the USMS project",
        Contact = new OpenApiContact
        {
            Name = "Support Team",
            Email = "support@example.com",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://example.com/license")
        }
    });
});

// Register dependencies via extension method
builder.Services.ConfigureDependencyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Sử dụng Swagger và SwaggerUI trong môi trường Development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Định nghĩa Swagger endpoint chính xác
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SchedulerService API V1");
        // Sử dụng đường dẫn mặc định "/swagger" cho UI
        c.RoutePrefix = "swagger"; // Sử dụng đường dẫn https://localhost:7131/swagger
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
