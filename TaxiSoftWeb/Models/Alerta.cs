using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiSoftWeb.Models;

public partial class Alerta
{
    public int IdAlerta { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    public DateTime? FechaDesde { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
    public DateTime? FechaHasta { get; set; }

    public int? DiasAnticipacion { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstadoA { get; set; }

    public virtual EstadosActividade? IdEstadoANavigation { get; set; }
}
