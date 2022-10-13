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
    public class CarnetsController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public CarnetsController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Carnets
        public async Task<IActionResult> Index()
        {
              return View(await _context.Carnets.ToListAsync());
        }

        // GET: Carnets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carnets == null)
            {
                return NotFound();
            }

            var carnet = await _context.Carnets
                .FirstOrDefaultAsync(m => m.IdCarnet == id);
            if (carnet == null)
            {
                return NotFound();
            }

            return View(carnet);
        }

        // GET: Carnets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carnets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarnet,NroCarnet,VtoCarnet")] Carnet carnet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carnet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carnet);
        }

        // GET: Carnets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carnets == null)
            {
                return NotFound();
            }

            var carnet = await _context.Carnets.FindAsync(id);
            if (carnet == null)
            {
                return NotFound();
            }
            return View(carnet);
        }

        // POST: Carnets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCarnet,NroCarnet,VtoCarnet")] Carnet carnet)
        {
            if (id != carnet.IdCarnet)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carnet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarnetExists(carnet.IdCarnet))
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
            return View(carnet);
        }

        // GET: Carnets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carnets == null)
            {
                return NotFound();
            }

            var carnet = await _context.Carnets
                .FirstOrDefaultAsync(m => m.IdCarnet == id);
            if (carnet == null)
            {
                return NotFound();
            }

            return View(carnet);
        }

        // POST: Carnets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carnets == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Carnets'  is null.");
            }
            var carnet = await _context.Carnets.FindAsync(id);
            if (carnet != null)
            {
                _context.Carnets.Remove(carnet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarnetExists(int id)
        {
          return _context.Carnets.Any(e => e.IdCarnet == id);
        }
    }
}
