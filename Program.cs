using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SanayiCebimdeBackend.Application.Interfaces;
using SanayiCebimdeBackend.Application.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Güvenli CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "https://sanayicebimde.net",
                "https://www.sanayicebimde.net",
                "http://localhost:3000" // Development için
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Authentication için gerekli
    });
});

builder.Services.AddDbContext<SanayiCebimdeBackend.Infrastructure.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Administrator"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager", "Admin"));
    options.AddPolicy("TechnicianOnly", policy => policy.RequireRole("Technician", "Tech", "Admin"));
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();