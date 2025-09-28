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

// G�venli CORS policy tan�m�
// Not: AllowCredentials kullan�ld��� i�in WithOrigins ile t�m kaynaklara (*) izin verilemez.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "https://sanayicebimde.net",       // Yay�n adresi
                "https://www.sanayicebimde.net",    // Yay�n adresi (www'lu)
                "http://localhost:3000"           // Geli�tirme ortam�
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Cookie/Authentication ba�l�klar� i�in gerekli
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

// Scope Tan�mlar�
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUstalarService, UstalarService>();

// Yetkilendirme Politikalar�
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin", "Administrator"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager", "Admin"));
    options.AddPolicy("TechnicianOnly", policy => policy.RequireRole("Technician", "Tech", "Admin"));
});


var app = builder.Build();

// ** 2. Middleware Yap�land�rmas� **

// Development ortam�nda Swagger'� a�
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

// 1. Routing'i etkinle�tir.
// Bu, gelen iste�in hangi Controller/Endpoint'e y�nlendirilece�ini bilmemizi sa�lar.
app.UseRouting();

// 2. CORS'u etkinle�tir ve politikan�z� uygulay�n.
// Do�ru �al��t���ndan emin olmak i�in UseRouting'den hemen sonra gelmelidir.
app.UseCors("AllowSpecificOrigins");

// 3. Authentication (Kullan�c� Kimli�ini belirleme)
app.UseAuthentication();

// 4. Authorization (Kullan�c� yetkisini kontrol etme)
app.UseAuthorization();

// 5. Endpoint'leri haritala.
app.MapControllers();

app.Run();