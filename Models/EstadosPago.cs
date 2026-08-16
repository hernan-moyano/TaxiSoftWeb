using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class EstadosPago
{
    public int IdEstadoP { get; set; }

    public string? NomEstadoP { get; set; }

    public virtual ICollection<Impuesto> Impuestos { get; } = new List<Impuesto>();

    public virtual ICollection<Multa> Multa { get; } = new List<Multa>();
}
