using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Itv
{
    public int IdItv { get; set; }

    public DateTime? VigenciaDesde { get; set; }

    public DateTime? VigenciaHasta { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Valor { get; set; }

    public int? IdVehiculo { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
