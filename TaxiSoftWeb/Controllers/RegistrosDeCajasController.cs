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
    public class RegistrosDeCajasController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public RegistrosDeCajasController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: RegistrosDeCajas
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.RegistrosDeCajas.Include(r => r.CuilNavigation).Include(r => r.IdCajaNavigation).Include(r => r.IdOperacionNavigation).Include(r => r.IdTurnoNavigation).Include(r => r.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: RegistrosDeCajas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RegistrosDeCajas == null)
            {
                return NotFound();
            }

            var registrosDeCaja = await _context.RegistrosDeCajas
                .Include(r => r.CuilNavigation)
                .Include(r => r.IdCajaNavigation)
                .Include(r => r.IdOperacionNavigation)
                .Include(r => r.IdTurnoNavigation)
                .Include(r => r.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdRegistroCaja == id);
            if (registrosDeCaja == null)
            {
                return NotFound();
            }

            return View(registrosDeCaja);
        }

        // GET: RegistrosDeCajas/Create
        public IActionResult Create()
        {
            ViewData["Cuil"] = new SelectList(_context.Conductores, "Cuil", "Cuil");
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja");
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion");
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo");
            return View();
        }

        // POST: RegistrosDeCajas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistroCaja,FechaRegisCaja,Concepto,Importe,IdTurno,Cuil,IdVehiculo,IdCaja,IdOperacion")] RegistrosDeCaja registrosDeCaja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registrosDeCaja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cuil"] = new SelectList(_context.Conductores, "Cuil", "Cuil", registrosDeCaja.Cuil);
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja", registrosDeCaja.IdCaja);
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion", registrosDeCaja.IdOperacion);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", registrosDeCaja.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", registrosDeCaja.IdVehiculo);
            return View(registrosDeCaja);
        }

        // GET: RegistrosDeCajas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RegistrosDeCajas == null)
            {
                return NotFound();
            }

            var registrosDeCaja = await _context.RegistrosDeCajas.FindAsync(id);
            if (registrosDeCaja == null)
            {
                return NotFound();
            }
            ViewData["Cuil"] = new SelectList(_context.Conductores, "Cuil", "Cuil", registrosDeCaja.Cuil);
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja", registrosDeCaja.IdCaja);
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion", registrosDeCaja.IdOperacion);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", registrosDeCaja.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", registrosDeCaja.IdVehiculo);
            return View(registrosDeCaja);
        }

        // POST: RegistrosDeCajas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRegistroCaja,FechaRegisCaja,Concepto,Importe,IdTurno,Cuil,IdVehiculo,IdCaja,IdOperacion")] RegistrosDeCaja registrosDeCaja)
        {
            if (id != registrosDeCaja.IdRegistroCaja)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrosDeCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrosDeCajaExists(registrosDeCaja.IdRegistroCaja))
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
            ViewData["Cuil"] = new SelectList(_context.Conductores, "Cuil", "Cuil", registrosDeCaja.Cuil);
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja", registrosDeCaja.IdCaja);
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion", registrosDeCaja.IdOperacion);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", registrosDeCaja.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", registrosDeCaja.IdVehiculo);
            return View(registrosDeCaja);
        }

        // GET: RegistrosDeCajas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RegistrosDeCajas == null)
            {
                return NotFound();
            }

            var registrosDeCaja = await _context.RegistrosDeCajas
                .Include(r => r.CuilNavigation)
                .Include(r => r.IdCajaNavigation)
                .Include(r => r.IdOperacionNavigation)
                .Include(r => r.IdTurnoNavigation)
                .Include(r => r.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdRegistroCaja == id);
            if (registrosDeCaja == null)
            {
                return NotFound();
            }

            return View(registrosDeCaja);
        }

        // POST: RegistrosDeCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RegistrosDeCajas == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.RegistrosDeCajas'  is null.");
            }
            var registrosDeCaja = await _context.RegistrosDeCajas.FindAsync(id);
            if (registrosDeCaja != null)
            {
                _context.RegistrosDeCajas.Remove(registrosDeCaja);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrosDeCajaExists(int id)
        {
          return _context.RegistrosDeCajas.Any(e => e.IdRegistroCaja == id);
        }
    }
}
