using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Carnet
{
    public int IdCarnet { get; set; }

    public string? NroCarnet { get; set; }

    public DateTime? VtoCarnet { get; set; }

    public virtual ICollection<Conductore> Conductores { get; } = new List<Conductore>();
}
