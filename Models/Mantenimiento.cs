using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaxiSoftWeb.Models;

public partial class Mantenimiento
{
    public int IdMant { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    public DateTime? FechaMant { get; set; }

    public string? Descripcion { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Valor { get; set; }

    public int? IdVehiculo { get; set; }

    public int? IdEstadoA { get; set; }

    public virtual EstadosActividade? IdEstadoANavigation { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
