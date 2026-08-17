using System;

namespace TaxiSoftWeb.Models;

public partial class AlertaAutomatica
{
    public int IdAlertaAuto { get; set; }

    public int? IdVehiculo { get; set; }

    public string? Cuil { get; set; }

    public string? TipoDocumento { get; set; }

    public int? IdRegistroOrigen { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public string? Descripcion { get; set; }

    public int? DiasAntelacion { get; set; }

    public bool? Activa { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }

    public virtual Conductore? CuilNavigation { get; set; }
}