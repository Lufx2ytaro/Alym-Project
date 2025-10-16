using Microsoft.AspNetCore.Mvc;
using Alym.Server.Data;
using Alym.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Alym.Server.Controllers
{
  //сообщает .NET, что этот класс — контроллер для REST API, и включает полезные функции (например, автоматическую проверку входных данных).
  //маршрут к твоему API. Так как контроллер называется TariffsController, URL будет:
    [ApiController]
    [Route("api/[controller]")]
    public class TariffsController : ControllerBase
  {
      //Тут происходит внедрение зависимостей (Dependency Injection).
      // .NET автоматически создаёт объект AppDbContext и передаёт его в контроллер.
      // То есть, когда сервер запускается, он создаёт фабрику контекстов — это такая "машина", которая при каждом запросе отдаёт тебе свежий экземпляр AppDbContext/
      // Это и есть фабричный подход: контроллер не создаёт контекст вручную (new AppDbContext()), а получает его уже готовым из сервиса, зарегистрированного в Program.cs.
        private readonly AppDbContext _db;
        public TariffsController(AppDbContext db) => _db = db;

    [HttpGet]
    //Этот метод обрабатывает запрос GET /api/tariffsи возвращает список всех тарифов из БД.
    // AsNoTracking() — ускоряет чтение данных, так как они не будут отслеживаться контекстом (удобно, 
    // если просто нужно вывести список, без редактирования).
    // ToListAsync() — асинхронно превращает результат в список.
    public async Task<IEnumerable<Tariff>> Get() =>
            await _db.Tariffs.AsNoTracking().ToListAsync();

        // Этот метод обрабатывает POST /api/tariffs и добавляет новый тариф в БД.
        // _db.Tariffs.Add(t) — добавляем объект в БД
        // await _db.SaveChangesAsync() — сохраняем изменения
        // CreatedAtAction(...) — возвращает ответ 201 (успешно создано) и добавленный объект

        [HttpPost]
        public async Task<IActionResult> Create(Tariff t)
        {
            _db.Tariffs.Add(t);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = t.Id }, t);
        }
    }
}//Фабрика! public TariffsController(AppDbContext db) => _db = db;
