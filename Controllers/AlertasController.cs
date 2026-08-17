using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;
using TaxiSoftWeb.ViewModels;

namespace TaxiSoftWeb.Controllers
{
    public class AlertasController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public AlertasController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Alertas
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.Alertas.Include(a => a.IdEstadoANavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Alertas/Vencimientos
        public async Task<IActionResult> Vencimientos(string tipoDocumento)
        {
            var consulta = _context.AlertasAutomaticas
                .Include(a => a.IdVehiculoNavigation)
                .Include(a => a.CuilNavigation)
                .Where(a => a.Activa == true);

            if (!string.IsNullOrEmpty(tipoDocumento))
            {
                consulta = consulta.Where(a => a.TipoDocumento == tipoDocumento);
            }

            var alertas = await consulta
                .OrderBy(a => a.FechaVencimiento)
                .ToListAsync();

            ViewData["TiposDocumento"] = new SelectList(
                new[] { "calibracion", "carnet", "impuesto", "itv", "seguro", "service" },
                tipoDocumento);

            return View(alertas);
        }

        // POST: Alertas/MarcarVencimiento/5
        [HttpPost, ActionName("MarcarVencimiento")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarVencimiento(int id)
        {
            var alerta = await _context.AlertasAutomaticas.FindAsync(id);
            if (alerta != null)
            {
                alerta.Activa = false;
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Vencimiento marcado como gestionado.";
            }
            return RedirectToAction(nameof(Vencimientos));
        }

        // GET: Alertas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alertas == null)
            {
                return NotFound();
            }

            var alerta = await _context.Alertas
                .Include(a => a.IdEstadoANavigation)
                .FirstOrDefaultAsync(m => m.IdAlerta == id);
            if (alerta == null)
            {
                return NotFound();
            }

            return View(alerta);
        }

        // GET: Alertas/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "NomEstadoA");
            return View();
        }

        // POST: Alertas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAlerta,FechaDesde,FechaHasta,DiasAnticipacion,Frecuencia,Descripcion,IdEstadoA")] Alerta alerta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alerta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "NomEstadoA", alerta.IdEstadoA);
            return View(alerta);
        }

        // GET: Alertas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alertas == null)
            {
                return NotFound();
            }

            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "NomEstadoA", alerta.IdEstadoA);
            return View(alerta);
        }

        // POST: Alertas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAlerta,FechaDesde,FechaHasta,DiasAnticipacion,Frecuencia,Descripcion,IdEstadoA")] Alerta alerta)
        {
            if (id != alerta.IdAlerta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alerta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlertaExists(alerta.IdAlerta))
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
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "NomEstadoA", alerta.IdEstadoA);
            return View(alerta);
        }

        // GET: Alertas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alertas == null)
            {
                return NotFound();
            }

            var alerta = await _context.Alertas
                .Include(a => a.IdEstadoANavigation)
                .FirstOrDefaultAsync(m => m.IdAlerta == id);
            if (alerta == null)
            {
                return NotFound();
            }

            return View(alerta);
        }

        // POST: Alertas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alertas == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Alertas'  is null.");
            }
            var alerta = await _context.Alertas.FindAsync(id);
            try
            {
                if (alerta != null)
                {
                    _context.Alertas.Remove(alerta);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["Mensaje"] = "No es posible eliminar el registro porque posee movimientos asociados.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AlertaExists(int id)
        {
          return _context.Alertas.Any(e => e.IdAlerta == id);
        }
    }
}
