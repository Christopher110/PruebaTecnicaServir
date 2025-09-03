using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaFullstack.Data;
using PruebaFullstack.Models;

namespace PruebaFullstack.Controllers
{
    public class DonacionController : Controller
    {
        private readonly AppDbContext _db;
        public DonacionController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(int? proyectoId)
        {
            var query = _db.Donaciones.Include(d => d.Proyecto).Include(d => d.Rubro).AsQueryable();
            if (proyectoId.HasValue) query = query.Where(d => d.ProyectoId == proyectoId);
            ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
            return View(await query.OrderByDescending(d => d.Fecha).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
            ViewBag.Rubros = await _db.Rubros.OrderBy(r => r.Nombre).ToListAsync();
            return View(new Donacion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Donacion model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
                ViewBag.Rubros = await _db.Rubros.OrderBy(r => r.Nombre).ToListAsync();
                return View(model);
            }
            _db.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Donación guardada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.Donaciones.FindAsync(id);
            if (entity == null) return NotFound();
            ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
            ViewBag.Rubros = await _db.Rubros.OrderBy(r => r.Nombre).ToListAsync();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Donacion model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid){
                ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
                ViewBag.Rubros = await _db.Rubros.OrderBy(r => r.Nombre).ToListAsync();
                return View(model);
            }
            _db.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Donación guardada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Donaciones.Include(d=>d.Proyecto).Include(d=>d.Rubro).FirstOrDefaultAsync(d=>d.Id==id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _db.Donaciones.FindAsync(id);
            if (entity == null) return NotFound();
            _db.Donaciones.Remove(entity);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Donación eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
