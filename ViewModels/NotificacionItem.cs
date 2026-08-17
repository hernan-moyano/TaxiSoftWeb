using System;

namespace TaxiSoftWeb.ViewModels;

public class NotificacionItem
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public string? Tipo { get; set; }

    public string? EsquemaColor
    {
        get
        {
            if (!FechaVencimiento.HasValue)
            {
                return "bg-info";
            }
            var dias = (FechaVencimiento.Value.Date - DateTime.Today).Days;
            if (dias < 0)
            {
                return "bg-danger";
            }
            if (dias <= 15)
            {
                return "bg-warning";
            }
            return "bg-success";
        }
    }
}