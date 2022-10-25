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
    public class ConductoresController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public ConductoresController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Conductores
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.Conductores.Include(c => c.IdCarnetNavigation).Include(c => c.IdDomicilioNavigation).Include(c => c.IdPuestoNavigation).Include(c => c.IdTurnoNavigation).Include(c => c.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Conductores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Conductores == null)
            {
                return NotFound();
            }

            var conductore = await _context.Conductores
                .Include(c => c.IdCarnetNavigation)
                .Include(c => c.IdDomicilioNavigation)
                .Include(c => c.IdPuestoNavigation)
                .Include(c => c.IdTurnoNavigation)
                .Include(c => c.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.Cuil == id);
            if (conductore == null)
            {
                return NotFound();
            }

            return View(conductore);
        }

        // GET: Conductores/Create
        public IActionResult Create()
        {
            ViewData["IdCarnet"] = new SelectList(_context.Carnets, "IdCarnet", "NroCarnet");
            ViewData["IdDomicilio"] = new SelectList(_context.Domicilios, "IdDomicilio", "IdDomicilio");
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "TarDesepeniada");
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo");
            return View();
        }

        // POST: Conductores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cuil,Dni,Apellido,Nombre,FechaNacimiento,Telefono,IdDomicilio,IdCarnet,IdPuesto,IdTurno,Activo,IdVehiculo,IdCarnetNavigation, IdDomicilioNavigation")] Conductore conductore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conductore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCarnet"] = new SelectList(_context.Carnets, "IdCarnet", "NroCarnet", conductore.IdCarnet);
            ViewData["IdDomicilio"] = new SelectList(_context.Domicilios, "IdDomicilio", "IdDomicilio", conductore.IdDomicilio);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "TarDesepeniada", conductore.IdPuesto);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", conductore.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", conductore.IdVehiculo);
            return View(conductore);
        }

        // GET: Conductores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Conductores == null)
            {
                return NotFound();
            }
            var conductore = await _context.Conductores.FindAsync(id);          
            if (conductore == null)
            {
                return NotFound();
            }
            var domicilio = await _context.Domicilios.FindAsync(conductore.IdDomicilio);
            var carnet = await _context.Carnets.FindAsync(conductore.IdCarnet);
            ViewData["IdCarnet"] = new SelectList(_context.Carnets, "IdCarnet", "IdCarnet", carnet);
            ViewData["IdDomicilio"] = new SelectList(_context.Domicilios, "IdDomicilio", "IdDomicilio", domicilio);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "TarDesepeniada", conductore.IdPuesto);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", conductore.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", conductore.IdVehiculo);
            return View(conductore);
        }
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null || _context.Conductores == null)
        //    {
        //        return NotFound();
        //    }
        //    var conductore = await _context.Conductores.FindAsync(id);
        //    if (conductore == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdCarnet"] = new SelectList(_context.Carnets, "IdCarnet", "NroCarnet", conductore.IdCarnet);
        //    ViewData["IdDomicilio"] = new SelectList(_context.Domicilios, "IdDomicilio", "IdDomicilio", conductore.IdDomicilio);
        //    ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "TarDesepeniada", conductore.IdPuesto);
        //    ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", conductore.IdTurno);
        //    ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", conductore.IdVehiculo);
        //    return View(conductore);
        //}

        // POST: Conductores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
        Edit(string id, 
        [Bind("Cuil,Dni,Apellido,Nombre,FechaNacimiento,Telefono,IdDomicilio,IdCarnet,IdPuesto,IdTurno,Activo,IdVehiculo")] Conductore conductore)
        {
            if (id != conductore.Cuil)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conductore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConductoreExists(conductore.Cuil))
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
            ViewData["IdCarnet"] = new SelectList(_context.Carnets, "IdCarnet", "IdCarnet", conductore.IdCarnet);
            ViewData["IdDomicilio"] = new SelectList(_context.Domicilios, "IdDomicilio", "IdDomicilio", conductore.IdDomicilio);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "TarDesepeniada", conductore.IdPuesto);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", conductore.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", conductore.IdVehiculo);
            return View(conductore);
        }

        // GET: Conductores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Conductores == null)
            {
                return NotFound();
            }

            var conductore = await _context.Conductores
                .Include(c => c.IdCarnetNavigation)
                .Include(c => c.IdDomicilioNavigation)
                .Include(c => c.IdPuestoNavigation)
                .Include(c => c.IdTurnoNavigation)
                .Include(c => c.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.Cuil == id);
            if (conductore == null)
            {
                return NotFound();
            }

            return View(conductore);
        }

        // POST: Conductores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Conductores == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Conductores'  is null.");
            }
            var conductore = await _context.Conductores.FindAsync(id);
            if (conductore != null)
            {
                var domicilio = await _context.Domicilios.FindAsync(conductore.IdDomicilio);
                if (domicilio != null)
                {
                    _context.Domicilios.Remove(domicilio);
                }
                await _context.SaveChangesAsync();
                _context.Conductores.Remove(conductore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConductoreExists(string id)
        {
            return _context.Conductores.Any(e => e.Cuil == id);
        }

        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.Conductores == null)
        //    {
        //        return Problem("Entity set 'TaxisoftDbContext.Conductores'  is null.");
        //    }
        //    var conductore = await _context.Conductores.FindAsync(id);
        //    if (conductore != null)
        //    {
        //        _context.Conductores.Remove(conductore);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ConductoreExists(string id)
        //{
        //  return _context.Conductores.Any(e => e.Cuil == id);
        //}
    }
}
