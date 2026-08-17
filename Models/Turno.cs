using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Turno
{
    public int IdTurno { get; set; }

    public string? NomTurno { get; set; }

    public TimeSpan? HoraInicio { get; set; }

    public TimeSpan? HoraFin { get; set; }

    public virtual ICollection<Conductore> Conductores { get; } = new List<Conductore>();

    public virtual ICollection<RegistrosDeCaja> RegistrosDeCajas { get; } = new List<RegistrosDeCaja>();
}
