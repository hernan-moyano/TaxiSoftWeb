using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;
using TaxiSoftWeb.ViewModels;

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

            var alertasManuales = await _context.Alertas
                .AsNoTracking()
                .Include(a => a.IdEstadoANavigation)
                .Where(a => a.IdEstadoA == 1)
                .Where(a => a.FechaDesde == null || a.FechaDesde <= fechaActual)
                .Where(a => a.FechaHasta == null || a.FechaHasta >= fechaActual)
                .Where(a => a.DiasAnticipacion == null || a.FechaHasta == null
                            || a.FechaHasta.Value.AddDays(-a.DiasAnticipacion.Value) <= fechaActual)
                .OrderBy(a => a.FechaHasta)
                .ToListAsync();

            var alertasAutomaticas = await _context.AlertasAutomaticas
                .AsNoTracking()
                .Where(a => a.Activa == true)
                .OrderBy(a => a.FechaVencimiento)
                .ToListAsync();

            var items = alertasManuales
                .Select(a => new NotificacionItem
                {
                    Id = a.IdAlerta,
                    Descripcion = a.Descripcion,
                    FechaVencimiento = a.FechaHasta,
                    Tipo = "manual"
                })
                .Concat(alertasAutomaticas.Select(a => new NotificacionItem
                {
                    Id = a.IdAlertaAuto,
                    Descripcion = a.Descripcion,
                    FechaVencimiento = a.FechaVencimiento,
                    Tipo = a.TipoDocumento
                }))
                .OrderBy(n => n.FechaVencimiento)
                .Take(10)
                .ToList();

            return View(items);
        }
    }
}