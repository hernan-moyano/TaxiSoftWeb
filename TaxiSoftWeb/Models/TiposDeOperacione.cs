using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class TiposDeOperacione
{
    public int IdOperacion { get; set; }

    public string? NomOperacion { get; set; }

    public virtual ICollection<RegistrosDeCaja> RegistrosDeCajas { get; } = new List<RegistrosDeCaja>();
}
