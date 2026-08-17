namespace TaxiSoftWeb.ViewModels;

public class ReporteMargenVehiculo
{
    public int? IdVehiculo { get; set; }

    public string? Patente { get; set; }

    public string? Marca { get; set; }

    public string? Modelo { get; set; }

    public string? Chofer { get; set; }

    public decimal TotalIngresos { get; set; }

    public decimal TotalEgresos { get; set; }

    public decimal MargenNeto { get; set; }

    public decimal PorcentajeMargen { get; set; }
}