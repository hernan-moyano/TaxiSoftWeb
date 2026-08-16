using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;

namespace TaxiSoftWeb.Controllers
{
    public class TiposDeCajasController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public TiposDeCajasController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: TiposDeCajas
        public async Task<IActionResult> Index()
        {
              return View(await _context.TiposDeCajas.ToListAsync());
        }

        // GET: TiposDeCajas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TiposDeCajas == null)
            {
                return NotFound();
            }

            var tiposDeCaja = await _context.TiposDeCajas
                .FirstOrDefaultAsync(m => m.IdCaja == id);
            if (tiposDeCaja == null)
            {
                return NotFound();
            }

            return View(tiposDeCaja);
        }

        // GET: TiposDeCajas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TiposDeCajas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCaja,NomCaja,Descripcion,Activo")] TiposDeCaja tiposDeCaja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tiposDeCaja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tiposDeCaja);
        }

        // GET: TiposDeCajas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TiposDeCajas == null)
            {
                return NotFound();
            }

            var tiposDeCaja = await _context.TiposDeCajas.FindAsync(id);
            if (tiposDeCaja == null)
            {
                return NotFound();
            }
            return View(tiposDeCaja);
        }

        // POST: TiposDeCajas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCaja,NomCaja,Descripcion,Activo")] TiposDeCaja tiposDeCaja)
        {
            if (id != tiposDeCaja.IdCaja)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tiposDeCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiposDeCajaExists(tiposDeCaja.IdCaja))
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
            return View(tiposDeCaja);
        }

        // GET: TiposDeCajas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TiposDeCajas == null)
            {
                return NotFound();
            }

            var tiposDeCaja = await _context.TiposDeCajas
                .FirstOrDefaultAsync(m => m.IdCaja == id);
            if (tiposDeCaja == null)
            {
                return NotFound();
            }

            return View(tiposDeCaja);
        }

        // POST: TiposDeCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TiposDeCajas == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.TiposDeCajas'  is null.");
            }
            var tiposDeCaja = await _context.TiposDeCajas.FindAsync(id);
            try {
                if (tiposDeCaja != null)
                {
                    _context.TiposDeCajas.Remove(tiposDeCaja);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["Mensaje"] = $"No es posible eliminar la caja {tiposDeCaja.NomCaja}, por poseer registros asociados";
            }            
            return RedirectToAction(nameof(Index));
        }

        private bool TiposDeCajaExists(int id)
        {
          return _context.TiposDeCajas.Any(e => e.IdCaja == id);
        }
    }
}
