using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessObject.AppDBContext;
using Core;
using Microsoft.EntityFrameworkCore;
using UserService.Repository.UserRepository;
using UserService.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Lấy cấu hình JWT từ appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// 🔹 Cấu hình Authentication sử dụng JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Nếu deploy trên HTTPS, nên đặt thành true
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true, // Xác thực thời gian hết hạn của token
            ClockSkew = TimeSpan.Zero // Không cho phép thời gian trễ
        };
    });

// 🔹 Đăng ký Authorization
builder.Services.AddAuthorization();
builder.Services.AddScoped<UserRepository>();  // Register UserRepository
builder.Services.AddScoped<LoginService>();

// 🔹 Đăng ký CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// 🔹 Đăng ký Controllers, Swagger, Dependency Injection
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDependencyInjection();

var app = builder.Build();

// 🔹 Cấu hình Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

// 🔹 Kích hoạt Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
