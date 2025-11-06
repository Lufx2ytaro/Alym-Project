using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Alym.Server.Data;
using Alym.Shared.Models;

namespace Alym.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TariffsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public TariffsController(AppDbContext db) => _db = db;

        [HttpGet("categories")]
        public async Task<IEnumerable<TariffCategory>> GetCategories() =>
            await _db.TariffCategories.AsNoTracking().ToListAsync();

        [HttpGet("regions")]
        public async Task<IEnumerable<Region>> GetRegions() =>
            await _db.Regions.AsNoTracking().ToListAsync();

        [HttpGet]
        public async Task<IEnumerable<Tariff>> GetTariffs([FromQuery] int? categoryId, [FromQuery] int? regionId)
        {
            var q = _db.Tariffs.Include(t => t.TariffCategory).Include(t => t.Region).AsQueryable();

            if (categoryId.HasValue) q = q.Where(t => t.TariffCategoryId == categoryId.Value);
            if (regionId.HasValue) q = q.Where(t => t.RegionId == regionId.Value);

            return await q.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tariff>> GetById(int id)
        {
            var t = await _db.Tariffs.Include(x => x.Region).Include(x => x.TariffCategory)
                                     .FirstOrDefaultAsync(x => x.Id == id);
            if (t == null) return NotFound();
            return Ok(t);
        }

        // (опционально) POST/PUT/DELETE можно добавить позже для админки
    }
}
