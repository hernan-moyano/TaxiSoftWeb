using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class EstadosActividade
{
    public int IdEstadoA { get; set; }

    public string? NomEstadoA { get; set; }

    public virtual ICollection<Alerta> Alerta { get; } = new List<Alerta>();

    public virtual ICollection<Mantenimiento> Mantenimientos { get; } = new List<Mantenimiento>();
}
