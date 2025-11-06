using Microsoft.AspNetCore.Components.WebAssembly.Server;
using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.EntityFrameworkCore;
using Alym.Server.Data;
using Alym.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Подключаем Entity Framework Core (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tariffs.db"));

// ✅ CORS — разрешаем клиенту обращаться к серверу
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ✅ Контроллеры и Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Создание и инициализация базы данных (seed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // ---- Заполняем таблицы по умолчанию ----
    if (!db.TariffCategories.Any())
    {
        db.TariffCategories.AddRange(new[]
        {
            new TariffCategory { Name = "Электричество" },
            new TariffCategory { Name = "Вода" },
            new TariffCategory { Name = "Газ" },
            new TariffCategory { Name = "Отопление" }
        });
        db.SaveChanges();
    }

    if (!db.Regions.Any())
    {
        db.Regions.AddRange(new[]
        {
            new Region { Name = "Moscow" },
            new Region { Name = "Saint Petersburg" },
            new Region { Name = "Kazan" },
            new Region { Name = "Novosibirsk" }
        });
        db.SaveChanges();
    }

    if (!db.Tariffs.Any())
    {
        var elektId = db.TariffCategories.First(c => c.Name == "Электричество").Id;
        var waterId = db.TariffCategories.First(c => c.Name == "Вода").Id;

        var moscowId = db.Regions.First(r => r.Name == "Moscow").Id;
        var spbId = db.Regions.First(r => r.Name == "Saint Petersburg").Id;
        var kazId = db.Regions.First(r => r.Name == "Kazan").Id;

        db.Tariffs.AddRange(new[]
        {
            new Tariff { TariffCategoryId = elektId, RegionId = moscowId, PricePerUnit = 5.50m, Unit = "руб/кВт·ч", UpdatedAt = DateTime.UtcNow },
            new Tariff { TariffCategoryId = elektId, RegionId = spbId, PricePerUnit = 4.80m, Unit = "руб/кВт·ч", UpdatedAt = DateTime.UtcNow },
            new Tariff { TariffCategoryId = elektId, RegionId = kazId, PricePerUnit = 4.05m, Unit = "руб/кВт·ч", UpdatedAt = DateTime.UtcNow },

            new Tariff { TariffCategoryId = waterId, RegionId = moscowId, PricePerUnit = 50.0m, Unit = "руб/м³", UpdatedAt = DateTime.UtcNow },
            new Tariff { TariffCategoryId = waterId, RegionId = spbId, PricePerUnit = 45.0m, Unit = "руб/м³", UpdatedAt = DateTime.UtcNow }
        });
        db.SaveChanges();
    }

    // ✅ Создаём таблицу для бизнес-проектов, если она ещё не существует
    if (!db.Projects.Any())
    {
        db.Projects.Add(new Project
        {
            Name = "Пример проекта",
            BusinessType = "Кофейня",
            Description = "Демонстрационный проект для теста.",
            CreatedAt = DateTime.UtcNow,
            SavedData = "{}"
        });
        db.SaveChanges();
    }
}

// ✅ Swagger — только в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ✅ Middleware
app.UseHttpsRedirection();
app.UseCors();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

// ✅ Маршруты
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
