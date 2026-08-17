using System.Collections.Generic;

namespace TaxiSoftWeb.ViewModels;

public class ReporteDashboard
{
    public int FlotaActiva { get; set; }

    public int ChoferesActivos { get; set; }

    public decimal IngresosMes { get; set; }

    public decimal EgresosMes { get; set; }

    public decimal MargenMes { get; set; }

    public int VencimientosProximos { get; set; }

    public List<ReporteMargenVehiculo> TopVehiculosRentables { get; set; } = new();

    public List<ReporteMargenVehiculo> TopVehiculosConGastos { get; set; } = new();
}