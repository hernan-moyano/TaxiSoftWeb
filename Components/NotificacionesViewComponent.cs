using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;

namespace TaxiSoftWeb.Components
{
    public class NotificacionesViewComponent : ViewComponent
    {
        private readonly TaxisoftDbContext _context;

        public NotificacionesViewComponent(TaxisoftDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var fechaActual = DateTime.Now;

            var alertas = await _context.Alertas
                .AsNoTracking()
                .Include(a => a.IdEstadoANavigation)
                .Where(a => a.IdEstadoA == 1)
                .Where(a => a.FechaDesde == null || a.FechaDesde <= fechaActual)
                .Where(a => a.FechaHasta == null || a.FechaHasta >= fechaActual)
                .Where(a => a.DiasAnticipacion == null || a.FechaHasta == null
                            || a.FechaHasta.Value.AddDays(-a.DiasAnticipacion.Value) <= fechaActual)
                .OrderBy(a => a.FechaHasta)
                .Take(10)
                .ToListAsync();

            return View(alertas);
        }
    }
}
