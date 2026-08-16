using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Domicilio
{
    public int IdDomicilio { get; set; }

    public string? Calle { get; set; }

    public int? Numero { get; set; }

    public string? Piso { get; set; }

    public string? Departamento { get; set; }

    public string? Ciudad { get; set; }

    public virtual ICollection<Conductore> Conductores { get; } = new List<Conductore>();
}
