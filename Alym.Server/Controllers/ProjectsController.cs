using Microsoft.AspNetCore.Mvc;
using Alym.Server.Data;
using Alym.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Alym.Server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProjectsController : ControllerBase
  {
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
      _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Project>>> Get() =>
        await _context.Projects.OrderByDescending(p => p.CreatedAt).ToListAsync();

    [HttpPost]
    public async Task<ActionResult> Create(Project project)
    {
      _context.Projects.Add(project);
      await _context.SaveChangesAsync();
      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
      var project = await _context.Projects.FindAsync(id);
      if (project == null) return NotFound();
      _context.Projects.Remove(project);
      await _context.SaveChangesAsync();
      return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Project updated)
    {
      var project = await _context.Projects.FindAsync(id);
      if (project == null) return NotFound();

      project.Name = updated.Name;
      project.Description = updated.Description;
      project.Category = updated.Category;

      await _context.SaveChangesAsync();
      return Ok();
    }
  }
}
