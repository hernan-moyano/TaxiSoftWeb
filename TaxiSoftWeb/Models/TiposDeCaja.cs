using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class TiposDeCaja
{
    public int IdCaja { get; set; }

    public string? NomCaja { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<RegistrosDeCaja> RegistrosDeCajas { get; } = new List<RegistrosDeCaja>();
}
