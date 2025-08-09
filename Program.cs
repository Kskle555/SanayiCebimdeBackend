using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SanayiCebimdeBackend.Application.Interfaces;
using SanayiCebimdeBackend.Application.Services; // Add this if needed

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<SanayiCebimdeBackend.Infrastructure.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Configure validation parameters
        };
    });




// Register services
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//canlida swagger ui g�z�kmesi i�in a�a��daki sat�rlar� a�abiliriz sonra
//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
//});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("AllowLocalhost3000");
app.UseAuthorization();
app.MapControllers();

app.Run();