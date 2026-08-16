using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Conductore
{
    public string Cuil { get; set; } = null!;

    public string? Dni { get; set; }

    public string? Apellido { get; set; }

    public string? Nombre { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public string? Telefono { get; set; }

    public int? IdDomicilio { get; set; }

    public int? IdCarnet { get; set; }

    public int? IdPuesto { get; set; }

    public int? IdTurno { get; set; }

    public bool? Activo { get; set; }

    public int? IdVehiculo { get; set; }

    public virtual Carnet? IdCarnetNavigation { get; set; }

    public virtual Domicilio? IdDomicilioNavigation { get; set; }

    public virtual Puesto? IdPuestoNavigation { get; set; }

    public virtual Turno? IdTurnoNavigation { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }

    public virtual ICollection<RegistrosDeCaja> RegistrosDeCajas { get; } = new List<RegistrosDeCaja>();
}
