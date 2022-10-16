﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaxiSoftWeb.Models;

public partial class Multa
{
    public int IdMulta { get; set; }

    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    public DateTime? FechaVto { get; set; }

    public string? Descripcion { get; set; }

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Valor { get; set; }

    public int? IdVehiculo { get; set; }

    public int? IdEstadoP { get; set; }

    public virtual EstadosPago? IdEstadoPNavigation { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
