using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Alerta
{
    public int IdAlerta { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public int? DiasAnticipacion { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstadoA { get; set; }

    public virtual EstadosActividade? IdEstadoANavigation { get; set; }
}
