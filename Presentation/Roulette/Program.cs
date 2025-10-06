using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Roulette.Bussines.Interfaces;
using Roulette.Bussines.Services;
using Roulette.Infrastructure.Data;
using Roulette.Infrastructure.Interfaces;
using Roulette.Infrastructure.Repositories;
using Roulette.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Configuración de base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Inyección de dependencias
builder.Services.AddScoped<IDBRepositoryEF, DBRepositoryEF>();
builder.Services.AddScoped<IUserService, UserService>();

// Controladores y endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger con header "User-Id"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Roulette API",
        Version = "v1"
    });

    c.AddSecurityDefinition("User-Id", new OpenApiSecurityScheme
    {
        Name = "User-Id",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "User ID requerido en el header para realizar apuestas"
    });

    c.OperationFilter<AddRequiredHeaderParameter>();
});

var app = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error aplicando migraciones: {ex.Message}");
    }
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Roulette API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
