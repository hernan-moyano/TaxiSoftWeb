using System;
using System.Collections.Generic;
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
                .ToListAsync();

            var alertasAutomaticas = await _context.AlertasAutomaticas
                .AsNoTracking()
                .Where(a => a.Activa == true)
                .OrderBy(a => a.FechaVencimiento)
                .ToListAsync();

            var itemsManuales = new List<NotificacionItem>();

            foreach (var a in alertasManuales)
            {
                var proximaOcurrencia = ProximaOcurrencia(a, fechaActual);
                if (!proximaOcurrencia.HasValue)
                {
                    continue;
                }

                var vence = proximaOcurrencia.Value;
                if (a.FechaDesde != null && vence < a.FechaDesde.Value)
                {
                    continue;
                }
                if (a.FechaHasta != null && vence > a.FechaHasta.Value)
                {
                    continue;
                }
                if (a.DiasAnticipacion != null && vence.AddDays(-a.DiasAnticipacion.Value) > fechaActual)
                {
                    continue;
                }

                itemsManuales.Add(new NotificacionItem
                {
                    Id = a.IdAlerta,
                    Descripcion = a.Descripcion,
                    FechaVencimiento = vence,
                    Tipo = "manual"
                });
            }

            var items = itemsManuales
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

        private static DateTime? ProximaOcurrencia(Alerta a, DateTime fechaActual)
        {
            var desde = a.FechaDesde ?? fechaActual;
            var frecuencia = string.IsNullOrWhiteSpace(a.Frecuencia) ? "una_vez" : a.Frecuencia;

            if (frecuencia == "una_vez")
            {
                return desde;
            }

            int dias;
            switch (frecuencia)
            {
                case "semanal":
                    dias = 7;
                    break;
                case "quincenal":
                    dias = 15;
                    break;
                case "mensual":
                    dias = 30;
                    break;
                case "bimestral":
                    dias = 60;
                    break;
                case "trimestral":
                    dias = 90;
                    break;
                case "anual":
                    dias = 365;
                    break;
                default:
                    return desde;
            }

            var ocurrencia = desde;
            while (ocurrencia < fechaActual.Date)
            {
                ocurrencia = ocurrencia.AddDays(dias);
            }

            if (a.FechaHasta != null && ocurrencia > a.FechaHasta.Value)
            {
                return null;
            }

            return ocurrencia;
        }
    }
}