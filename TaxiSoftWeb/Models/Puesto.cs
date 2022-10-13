using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Puesto
{
    public int IdPuesto { get; set; }

    public string? CalifProfesional { get; set; }

    public string? TarDesepeniada { get; set; }

    public virtual ICollection<Conductore> Conductores { get; } = new List<Conductore>();
}
