using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Ocelot for API Gateway
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true); // Load Ocelot configuration
builder.Services.AddOcelot(); // Register Ocelot services
builder.Services.AddCors(); // Register CORS services

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(cors =>
    cors.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseOcelot().Wait();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
