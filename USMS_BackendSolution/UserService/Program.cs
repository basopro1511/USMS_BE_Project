using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessObject.AppDBContext;
using Core;
using Microsoft.EntityFrameworkCore;
using UserService.Repository.UserRepository;
using UserService.Services.UserService;
using Microsoft.OpenApi.Models;
using Authorization.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// 🔹 Lấy cấu hình JWT từ appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddCommonAuthorization();
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
// 🔹 Cấu hình Authentication sử dụng JWT
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service API", Version = "v1" });
    // 🔹 Thêm ô nhập Token (Bearer Authentication)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập Token theo định dạng: Bearer {your_token}"
    });
    // 🔹 Tự động thêm Authorization vào tất cả các request
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
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
builder.Services.AddScoped<UserService.Services.UserService.UserService>();
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