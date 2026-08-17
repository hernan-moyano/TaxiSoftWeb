namespace TaxiSoftWeb.ViewModels;

public class LiquidacionChofer
{
    public string? Cuil { get; set; }

    public string? NombreChofer { get; set; }

    public string? Patente { get; set; }

    public decimal TotalIngresos { get; set; }

    public decimal TotalEgresos { get; set; }

    public decimal RecaudacionNeta { get; set; }

    public string? TipoAlquiler { get; set; }

    public decimal? Alquiler { get; set; }

    public decimal? Comision { get; set; }
}