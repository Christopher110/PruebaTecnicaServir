using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaFullstack.Data;
using PruebaFullstack.Models;

namespace PruebaFullstack.Controllers
{
    public class RubroController : Controller
    {
        private readonly AppDbContext _db;
        public RubroController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(int? proyectoId)
        {
            var query = _db.Rubros.Include(r => r.Proyecto).AsQueryable();
            if (proyectoId.HasValue) query = query.Where(r => r.ProyectoId == proyectoId);
            var list = await query.OrderBy(r => r.ProyectoId).ThenBy(r => r.Nombre).ToListAsync();
            ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
            return View(new Rubro());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rubro model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
                return View(model);
            }
            try
            {
                _db.Add(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Rubro guardado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Ya existe un Rubro con ese nombre en el proyecto seleccionado.");
                TempData["Error"] = "Ya existe un Rubro con ese nombre en el proyecto seleccionado.";
                ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.Rubros.FindAsync(id);
            if (entity == null) return NotFound();
            ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rubro model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid){
                ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
                return View(model);
            }
            try{
                _db.Update(model);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Rubro guardado correctamente.";
                return RedirectToAction(nameof(Index));
            }catch (DbUpdateException){
                ModelState.AddModelError("", "Ya existe un Rubro con ese nombre en el proyecto seleccionado.");
                TempData["Error"] = "Ya existe un Rubro con ese nombre en el proyecto seleccionado.";
                ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.Rubros.Include(r=>r.Proyecto).FirstOrDefaultAsync(r=>r.Id==id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _db.Rubros.FindAsync(id);
            if (entity == null) return NotFound();
            _db.Rubros.Remove(entity);
            try{
                await _db.SaveChangesAsync();
                TempData["Success"] = "Rubro eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }catch (DbUpdateException){
                TempData["Error"] = "No se puede eliminar: Rubro vinculado a Donaciones u Órdenes.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
