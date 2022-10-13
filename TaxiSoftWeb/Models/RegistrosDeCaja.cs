using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class RegistrosDeCaja
{
    public int IdRegistroCaja { get; set; }

    public DateTime? FechaRegisCaja { get; set; }

    public string? Concepto { get; set; }

    public decimal? Importe { get; set; }

    public int? IdTurno { get; set; }

    public string? Cuil { get; set; }

    public int? IdVehiculo { get; set; }

    public int? IdCaja { get; set; }

    public int? IdOperacion { get; set; }

    public virtual Conductore? CuilNavigation { get; set; }

    public virtual TiposDeCaja? IdCajaNavigation { get; set; }

    public virtual TiposDeOperacione? IdOperacionNavigation { get; set; }

    public virtual Turno? IdTurnoNavigation { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
