using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Impuesto
{
    public int IdImpuesto { get; set; }

    public DateTime? FechaVto { get; set; }

    public decimal? Valor { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstadoP { get; set; }

    public int? IdVehiculo { get; set; }

    public virtual EstadosPago? IdEstadoPNavigation { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
