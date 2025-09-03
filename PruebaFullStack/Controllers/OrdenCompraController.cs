using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaFullstack.Data;
using PruebaFullstack.Models;

namespace PruebaFullstack.Controllers
{
    public class OrdenCompraController : Controller
    {
        private readonly AppDbContext _db;
        public OrdenCompraController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var list = await _db.OrdenesCompra.Include(o => o.Detalles).ThenInclude(d=>d.Rubro)
                .OrderByDescending(o => o.Fecha).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
            ViewBag.Rubros = await _db.Rubros.OrderBy(r => r.Nombre).ToListAsync();
            return View(new OrdenCompra { Detalles = new List<OrdenCompraDetalle> { new OrdenCompraDetalle() } });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdenCompra model)
        {
            model.Detalles = model.Detalles.Where(d => d.Monto > 0 && d.ProyectoId > 0 && d.RubroId > 0).ToList();
            if (!ModelState.IsValid || model.Detalles.Count == 0)
            {
                ViewBag.Proyectos = await _db.Proyectos.OrderBy(p => p.Nombre).ToListAsync();
                ViewBag.Rubros = await _db.Rubros.OrderBy(r => r.Nombre).ToListAsync();
                ModelState.AddModelError("", "Debe ingresar al menos un detalle válido.");
                return View(model);
            }
            _db.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Orden creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _db.OrdenesCompra.Include(o=>o.Detalles).ThenInclude(d=>d.Rubro).FirstOrDefaultAsync(o=>o.Id==id);
            if (entity == null) return NotFound();
            return View(entity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _db.OrdenesCompra.FindAsync(id);
            if (entity == null) return NotFound();
            _db.OrdenesCompra.Remove(entity);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Orden eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
