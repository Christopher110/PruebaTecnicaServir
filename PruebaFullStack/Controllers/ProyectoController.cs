using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaFullstack.Data;
using PruebaFullstack.Models;

namespace PruebaFullstack.Controllers
{
    public class ProyectoController : Controller
    {
        private readonly AppDbContext _db;
        public ProyectoController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var list = await _db.Proyectos.OrderBy(p => p.Id).ToListAsync();
            return View(list);
        }

        public IActionResult Create() => View(new Proyecto());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proyecto model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Proyecto guardado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.Proyectos.FindAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proyecto model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Proyecto guardado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Proyectos.FindAsync(id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _db.Proyectos.FindAsync(id);
            if (entity == null) return NotFound();
            _db.Proyectos.Remove(entity);
            try
            {
                await _db.SaveChangesAsync();
                TempData["Success"] = "Proyecto eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "No se puede eliminar: existen registros relacionados (Rubros, Donaciones u Órdenes).";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
