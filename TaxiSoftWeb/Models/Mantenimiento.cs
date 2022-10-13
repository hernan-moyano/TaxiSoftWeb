using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Mantenimiento
{
    public int IdMant { get; set; }

    public DateTime? FechaMant { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Valor { get; set; }

    public int? IdVehiculo { get; set; }

    public int? IdEstadoA { get; set; }

    public virtual EstadosActividade? IdEstadoANavigation { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
