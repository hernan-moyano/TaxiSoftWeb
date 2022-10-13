using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Seguro
{
    public int IdSeguro { get; set; }

    public string? NroPoliza { get; set; }

    public string? Aseguradora { get; set; }

    public DateTime? VigenciaDesde { get; set; }

    public DateTime? VigenciaHasta { get; set; }

    public int? IdVehiculo { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
