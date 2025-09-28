using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SanayiCebimdeBackend.Application.Interfaces;
using SanayiCebimdeBackend.Application.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ** 1. Servis Ekleme **
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Güvenli CORS policy tanýmý
// Not: AllowCredentials kullanýldýðý için WithOrigins ile tüm kaynaklara (*) izin verilemez.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "https://sanayicebimde.net",       // Yayýn adresi
                "https://www.sanayicebimde.net",    // Yayýn adresi (www'lu)
                "http://localhost:3000"           // Geliþtirme ortamý
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Cookie/Authentication baþlýklarý için gerekli
    });
});

// DbContext Ekleme
builder.Services.AddDbContext<SanayiCebimdeBackend.Infrastructure.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication Servisi Ekleme (JWT)
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

// Scope Tanýmlarý
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUstalarService, UstalarService>();

// Yetkilendirme Politikalarý
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Administrator"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager", "Admin"));
    options.AddPolicy("TechnicianOnly", policy => policy.RequireRole("Technician", "Tech", "Admin"));
});


var app = builder.Build();

// ** 2. Middleware Yapýlandýrmasý **

// Development ortamýnda Swagger'ý aç
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

// 1. Routing'i etkinleþtir.
// Bu, gelen isteðin hangi Controller/Endpoint'e yönlendirileceðini bilmemizi saðlar.
app.UseRouting();

// 2. CORS'u etkinleþtir ve politikanýzý uygulayýn.
// Doðru çalýþtýðýndan emin olmak için UseRouting'den hemen sonra gelmelidir.
app.UseCors("AllowSpecificOrigins");

// 3. Authentication (Kullanýcý Kimliðini belirleme)
app.UseAuthentication();

// 4. Authorization (Kullanýcý yetkisini kontrol etme)
app.UseAuthorization();

// 5. Endpoint'leri haritala.
app.MapControllers();

app.Run();