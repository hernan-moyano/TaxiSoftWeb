using System;
using System.Collections.Generic;

namespace TaxiSoftWeb.Models;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    public string? Patente { get; set; }

    public string? Chapa { get; set; }

    public string? Marca { get; set; }

    public string? Modelo { get; set; }

    public int? Anio { get; set; }

    public decimal? Cilindraje { get; set; }

    public string? NroMotor { get; set; }

    public string? NroChasis { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Conductore> Conductores { get; } = new List<Conductore>();

    public virtual ICollection<Impuesto> Impuestos { get; } = new List<Impuesto>();

    public virtual ICollection<Itv> Itvs { get; } = new List<Itv>();

    public virtual ICollection<Mantenimiento> Mantenimientos { get; } = new List<Mantenimiento>();

    public virtual ICollection<Multa> Multa { get; } = new List<Multa>();

    public virtual ICollection<RegistrosDeCaja> RegistrosDeCajas { get; } = new List<RegistrosDeCaja>();

    public virtual ICollection<Seguro> Seguros { get; } = new List<Seguro>();
}
