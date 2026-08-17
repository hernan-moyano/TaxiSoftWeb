namespace TaxiSoftWeb.ViewModels;

public class ResumenGastoVehiculo
{
    public int? IdVehiculo { get; set; }

    public string? Patente { get; set; }

    public decimal Total { get; set; }

    public decimal TotalCombustible { get; set; }

    public decimal TotalLavados { get; set; }

    public decimal TotalNeumaticos { get; set; }

    public decimal TotalReparaciones { get; set; }

    public decimal TotalServices { get; set; }

    public decimal TotalImprevistos { get; set; }
}