using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;

namespace TaxiSoftWeb.Controllers
{
    public class TiposDeGastosController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public TiposDeGastosController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: TiposDeGastos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TiposDeGastos.ToListAsync());
        }

        // GET: TiposDeGastos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TiposDeGastos == null)
            {
                return NotFound();
            }

            var tipoGasto = await _context.TiposDeGastos
                .FirstOrDefaultAsync(m => m.IdTipoGasto == id);
            if (tipoGasto == null)
            {
                return NotFound();
            }

            return View(tipoGasto);
        }

        // GET: TiposDeGastos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposDeGastos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTipoGasto,Nombre,Descripcion,Activo")] TipoGasto tipoGasto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoGasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoGasto);
        }

        // GET: TiposDeGastos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TiposDeGastos == null)
            {
                return NotFound();
            }

            var tipoGasto = await _context.TiposDeGastos.FindAsync(id);
            if (tipoGasto == null)
            {
                return NotFound();
            }
            return View(tipoGasto);
        }

        // POST: TiposDeGastos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTipoGasto,Nombre,Descripcion,Activo")] TipoGasto tipoGasto)
        {
            if (id != tipoGasto.IdTipoGasto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoGasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoGastoExists(tipoGasto.IdTipoGasto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoGasto);
        }

        // GET: TiposDeGastos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TiposDeGastos == null)
            {
                return NotFound();
            }

            var tipoGasto = await _context.TiposDeGastos
                .FirstOrDefaultAsync(m => m.IdTipoGasto == id);
            if (tipoGasto == null)
            {
                return NotFound();
            }

            return View(tipoGasto);
        }

        // POST: TiposDeGastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TiposDeGastos == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.TiposDeGastos'  is null.");
            }
            var tipoGasto = await _context.TiposDeGastos.FindAsync(id);
            if (tipoGasto != null)
            {
                _context.TiposDeGastos.Remove(tipoGasto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoGastoExists(int id)
        {
            return _context.TiposDeGastos.Any(e => e.IdTipoGasto == id);
        }
    }
}