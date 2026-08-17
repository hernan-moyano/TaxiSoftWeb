using System;

namespace TaxiSoftWeb.Models;

public partial class CalibracionTaximetro
{
    public int IdCalibracion { get; set; }

    public int? IdVehiculo { get; set; }

    public DateTime? FechaCalibracion { get; set; }

    public DateTime? ProximaCalibracion { get; set; }

    public string? EntidadCalibradora { get; set; }

    public string? NroCertificado { get; set; }

    public decimal? Valor { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}