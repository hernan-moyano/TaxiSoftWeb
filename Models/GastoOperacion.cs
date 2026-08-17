using System;

namespace TaxiSoftWeb.Models;

public partial class GastoOperacion
{
    public int IdGasto { get; set; }

    public int? IdVehiculo { get; set; }

    public string? Cuil { get; set; }

    public DateTime? FechaGasto { get; set; }

    public int? IdTipoGasto { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Importe { get; set; }

    public int? Kilometraje { get; set; }

    public string? NroFactura { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }

    public virtual Conductore? CuilNavigation { get; set; }

    public virtual TipoGasto? IdTipoGastoNavigation { get; set; }
}