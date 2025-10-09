using Microsoft.AspNetCore.Mvc;
using Alym.Server.Data;
using Alym.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Alym.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TariffsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TariffsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Tariff>> Get() =>
            await _db.Tariffs.AsNoTracking().ToListAsync();

        [HttpPost]
        public async Task<IActionResult> Create(Tariff t)
        {
            _db.Tariffs.Add(t);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = t.Id }, t);
        }
    }
}
