using Microsoft.AspNetCore.Components.WebAssembly.Server;
using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.EntityFrameworkCore;
using Alym.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// EF / SQLite - Подключаем Entity Framework Core (ORM).
// Он связывает C# объекты с таблицами в SQLite.
// Файл tariffs.db — это база данных (создаётся автоматически).
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tariffs.db"));

// CORS (dev) Разрешаем CORS — чтобы клиент (веб) мог обращаться к серверу.
// Без этого браузер блокировал бы запросы с других доменов.
// МЕжсайтовый скриптинг 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Добавляем контроллеры (API) Говорим .NET, что будем использовать контроллеры —
// это классы, которые принимают и обрабатывают HTTP-запросы.
builder.Services.AddControllers();

// Swagger Подключаем Swagger — интерфейс для тестирования API прямо в браузере.
// (Доступен, когда app.Environment.IsDevelopment()).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Создаём само приложение (всё, что ты настроил — теперь превращается в сервер).
var app = builder.Build();

// Создаём БД и seed тарифов Этот блок создаёт базу данных при первом запуске
// и добавляет стартовые тарифы (seed), если таблица пустая.
// Так ты получаешь “данные по умолчанию”.
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
            new Alym.Shared.Models.Tariff { Region = "Kazan", Price = 3.05m },
            new Alym.Shared.Models.Tariff { Region = "Obninsk", Price = 4.20m }
        });
        db.SaveChanges();
    }
}

// Если приложение запущено в режиме разработки,
// включаем Swagger для тестов API через браузер.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//Это middleware — “слои обработки запросов”.
//UseHttpsRedirection() — перенаправляет всё на HTTPS.
//UseCors() — разрешает клиенту общаться с сервером.
//UseBlazorFrameworkFiles() — подключает Blazor-клиент.
//UseStaticFiles() — отдаёт файлы (CSS, JS, HTML).
//UseRouting() — включает маршрутизацию (чтобы контроллеры работали).

app.UseHttpsRedirection();
app.UseCors();

// Отдаём клиентские файлы (Blazor WASM hosted)
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

//Настраиваем маршруты:
//MapControllers() — говорит: “API обрабатываются контроллерами”.
//MapFallbackToFile("index.html") — если путь не найден —
//отдать клиенту (Blazor), чтобы SPA продолжила работать.

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
