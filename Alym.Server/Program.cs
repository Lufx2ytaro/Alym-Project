using Microsoft.AspNetCore.Components.WebAssembly.Server;
using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.EntityFrameworkCore;
using Alym.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// EF / SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tariffs.db"));

// CORS (dev)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Добавляем контроллеры (API)
builder.Services.AddControllers();

// Swagger (полезно в dev)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Создаём БД и seed тарифов (если пусто)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Tariffs.Any())
    {
        db.Tariffs.AddRange(new[]
        {
            new Alym.Shared.Models.Tariff { Region = "Moscow", Price = 5.50m },
            new Alym.Shared.Models.Tariff { Region = "Saint Petersburg", Price = 4.80m },
            new Alym.Shared.Models.Tariff { Region = "Novosibirsk", Price = 4.12m },
            new Alym.Shared.Models.Tariff { Region = "Kazan", Price = 4.05m },
            new Alym.Shared.Models.Tariff { Region = "Obninsk", Price = 4.20m }
        });
        db.SaveChanges();
    }
}

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Отдаём клиентские файлы (Blazor WASM hosted)
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
