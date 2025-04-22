using BusinessObject.AppDBContext;
using Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDependencyInjection();

// Register JWT Token
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata=false; // Nếu deploy trên HTTPS, nên đặt thành true
    options.SaveToken=true;
    options.TokenValidationParameters=new TokenValidationParameters
        {
        ValidateIssuerSigningKey=true,
        IssuerSigningKey=new SymmetricSecurityKey(key),
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidIssuer=jwtSettings["Issuer"],
        ValidAudience=jwtSettings["Audience"],
        ValidateLifetime=true, // Xác thực thời gian hết hạn của token
        ClockSkew=TimeSpan.Zero // Không cho phép thời gian trễ
        };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5174")
              .AllowAnyMethod()                    
              .AllowAnyHeader()                    
              .AllowCredentials();                 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Sử dụng CORS
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
