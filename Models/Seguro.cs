using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiSoftWeb.Models;

public partial class Seguro
{
    public int IdSeguro { get; set; }

    public string? NroPoliza { get; set; }

    public string? Aseguradora { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    public DateTime? VigenciaDesde { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    public DateTime? VigenciaHasta { get; set; }

    public int? IdVehiculo { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
