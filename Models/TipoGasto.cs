using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class TipoGasto
{
    public int IdTipoGasto { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<GastoOperacion> GastosOperaciones { get; } = new List<GastoOperacion>();
}