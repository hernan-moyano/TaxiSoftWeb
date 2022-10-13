using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaxiSoftWeb.Models;
    
public partial class TaxisoftDbContext : DbContext
{
    public TaxisoftDbContext()
    {
    }

    public TaxisoftDbContext(DbContextOptions<TaxisoftDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alerta> Alertas { get; set; }

    public virtual DbSet<Carnet> Carnets { get; set; }

    public virtual DbSet<Conductore> Conductores { get; set; }

    public virtual DbSet<Domicilio> Domicilios { get; set; }

    public virtual DbSet<EstadosActividade> EstadosActividades { get; set; }

    public virtual DbSet<EstadosPago> EstadosPagos { get; set; }

    public virtual DbSet<Impuesto> Impuestos { get; set; }

    public virtual DbSet<Itv> Itvs { get; set; }

    public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }

    public virtual DbSet<Multa> Multas { get; set; }

    public virtual DbSet<Puesto> Puestos { get; set; }

    public virtual DbSet<RegistrosDeCaja> RegistrosDeCajas { get; set; }

    public virtual DbSet<Seguro> Seguros { get; set; }

    public virtual DbSet<TiposDeCaja> TiposDeCajas { get; set; }

    public virtual DbSet<TiposDeOperacione> TiposDeOperaciones { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Data Source=NB-HERNAN;Initial Catalog=TAXISOFT-DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alerta>(entity =>
        {
            entity.HasKey(e => e.IdAlerta).HasName("PK__Alertas__1227953E3A482B74");

            entity.Property(e => e.IdAlerta).HasColumnName("id_alerta");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.DiasAnticipacion).HasColumnName("diasAnticipacion");
            entity.Property(e => e.FechaDesde)
                .HasColumnType("datetime")
                .HasColumnName("fechaDesde");
            entity.Property(e => e.FechaHasta)
                .HasColumnType("datetime")
                .HasColumnName("fechaHasta");
            entity.Property(e => e.IdEstadoA).HasColumnName("Id_estadoA_");

            entity.HasOne(d => d.IdEstadoANavigation).WithMany(p => p.Alerta)
                .HasForeignKey(d => d.IdEstadoA)
                .HasConstraintName("FK__Alertas__Id_esta__571DF1D5");
        });

        modelBuilder.Entity<Carnet>(entity =>
        {
            entity.HasKey(e => e.IdCarnet).HasName("PK__Carnets__1688C19283540861");

            entity.Property(e => e.IdCarnet).HasColumnName("id_carnet");
            entity.Property(e => e.NroCarnet)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nroCarnet");
            entity.Property(e => e.VtoCarnet)
                .HasColumnType("date")
                .HasColumnName("vtoCarnet");
        });

        modelBuilder.Entity<Conductore>(entity =>
        {
            entity.HasKey(e => e.Cuil).HasName("PK__Conducto__2CDD98AE290E0992");

            entity.Property(e => e.Cuil)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("cuil");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("((1))")
                .HasColumnName("activo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("dni");
            entity.Property(e => e.FechaNacimiento)
                .HasColumnType("date")
                .HasColumnName("fechaNacimiento");
            entity.Property(e => e.IdCarnet).HasColumnName("id_carnet_");
            entity.Property(e => e.IdDomicilio).HasColumnName("id_domicilio_");
            entity.Property(e => e.IdPuesto).HasColumnName("id_puesto_");
            entity.Property(e => e.IdTurno).HasColumnName("id_turno_");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo_");
            entity.Property(e => e.Nombre)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdCarnetNavigation).WithMany(p => p.Conductores)
                .HasForeignKey(d => d.IdCarnet)
                .HasConstraintName("FK__Conductor__id_ca__3B75D760");

            entity.HasOne(d => d.IdDomicilioNavigation).WithMany(p => p.Conductores)
                .HasForeignKey(d => d.IdDomicilio)
                .HasConstraintName("FK__Conductor__id_do__3C69FB99");

            entity.HasOne(d => d.IdPuestoNavigation).WithMany(p => p.Conductores)
                .HasForeignKey(d => d.IdPuesto)
                .HasConstraintName("FK__Conductor__id_pu__3A81B327");

            entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.Conductores)
                .HasForeignKey(d => d.IdTurno)
                .HasConstraintName("FK__Conductor__id_tu__398D8EEE");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Conductores)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Conductor__id_ve__38996AB5");
        });

        modelBuilder.Entity<Domicilio>(entity =>
        {
            entity.HasKey(e => e.IdDomicilio).HasName("PK__Domicili__A0CCE5C2CB275F17");

            entity.Property(e => e.IdDomicilio).HasColumnName("id_domicilio");
            entity.Property(e => e.Calle)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.Departamento)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("departamento");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.Piso)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("piso");
        });

        modelBuilder.Entity<EstadosActividade>(entity =>
        {
            entity.HasKey(e => e.IdEstadoA).HasName("PK__EstadosA__6B32751C2CDF43A2");

            entity.Property(e => e.IdEstadoA).HasColumnName("Id_estadoA");
            entity.Property(e => e.NomEstadoA)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nomEstadoA");
        });

        modelBuilder.Entity<EstadosPago>(entity =>
        {
            entity.HasKey(e => e.IdEstadoP).HasName("PK__EstadosP__6B32750D8A15EC4E");

            entity.Property(e => e.IdEstadoP).HasColumnName("Id_estadoP");
            entity.Property(e => e.NomEstadoP)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("nomEstadoP");
        });

        modelBuilder.Entity<Impuesto>(entity =>
        {
            entity.HasKey(e => e.IdImpuesto).HasName("PK__Impuesto__8546BDFCFB2927D6");

            entity.Property(e => e.IdImpuesto).HasColumnName("id_impuesto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaVto)
                .HasColumnType("date")
                .HasColumnName("fechaVto");
            entity.Property(e => e.IdEstadoP).HasColumnName("Id_estadoP_");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo_");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor");

            entity.HasOne(d => d.IdEstadoPNavigation).WithMany(p => p.Impuestos)
                .HasForeignKey(d => d.IdEstadoP)
                .HasConstraintName("FK__Impuestos__Id_es__5070F446");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Impuestos)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Impuestos__id_ve__4F7CD00D");
        });

        modelBuilder.Entity<Itv>(entity =>
        {
            entity.HasKey(e => e.IdItv).HasName("PK__ITVs__D62ADFEEB7B3E2D0");

            entity.ToTable("ITVs");

            entity.Property(e => e.IdItv).HasColumnName("id_itv");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo_");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor");
            entity.Property(e => e.VigenciaDesde)
                .HasColumnType("date")
                .HasColumnName("vigenciaDesde");
            entity.Property(e => e.VigenciaHasta)
                .HasColumnType("date")
                .HasColumnName("vigenciaHasta");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Itvs)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__ITVs__id_vehicul__48CFD27E");
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasKey(e => e.IdMant).HasName("PK__Mantenim__6FA198DEA4AFF90B");

            entity.Property(e => e.IdMant).HasColumnName("id_mant");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaMant)
                .HasColumnType("datetime")
                .HasColumnName("fechaMant");
            entity.Property(e => e.IdEstadoA).HasColumnName("Id_estadoA_");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo_");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor");

            entity.HasOne(d => d.IdEstadoANavigation).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.IdEstadoA)
                .HasConstraintName("FK__Mantenimi__Id_es__534D60F1");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Mantenimi__id_ve__5441852A");
        });

        modelBuilder.Entity<Multa>(entity =>
        {
            entity.HasKey(e => e.IdMulta).HasName("PK__Multas__295650BBD8300E64");

            entity.Property(e => e.IdMulta).HasColumnName("id_multa");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaVto)
                .HasColumnType("date")
                .HasColumnName("fechaVto");
            entity.Property(e => e.IdEstadoP).HasColumnName("Id_estadoP_");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo_");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor");

            entity.HasOne(d => d.IdEstadoPNavigation).WithMany(p => p.Multa)
                .HasForeignKey(d => d.IdEstadoP)
                .HasConstraintName("FK__Multas__Id_estad__4BAC3F29");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Multa)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Multas__id_vehic__4CA06362");
        });

        modelBuilder.Entity<Puesto>(entity =>
        {
            entity.HasKey(e => e.IdPuesto).HasName("PK__Puestos__123AAB99807408D8");

            entity.Property(e => e.IdPuesto).HasColumnName("id_puesto");
            entity.Property(e => e.CalifProfesional)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("califProfesional");
            entity.Property(e => e.TarDesepeniada)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("tarDesepeniada");
        });

        modelBuilder.Entity<RegistrosDeCaja>(entity =>
        {
            entity.HasKey(e => e.IdRegistroCaja).HasName("PK__Registro__53E7860F39F615BE");

            entity.Property(e => e.IdRegistroCaja).HasColumnName("id_registroCaja");
            entity.Property(e => e.Concepto)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("concepto");
            entity.Property(e => e.Cuil)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("cuil_");
            entity.Property(e => e.FechaRegisCaja)
                .HasColumnType("datetime")
                .HasColumnName("fechaRegisCaja");
            entity.Property(e => e.IdCaja).HasColumnName("id_caja_");
            entity.Property(e => e.IdOperacion).HasColumnName("id_Operacion_");
            entity.Property(e => e.IdTurno).HasColumnName("id_turno_");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo_");
            entity.Property(e => e.Importe)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("importe");

            entity.HasOne(d => d.CuilNavigation).WithMany(p => p.RegistrosDeCajas)
                .HasForeignKey(d => d.Cuil)
                .HasConstraintName("FK__Registros__cuil___4316F928");

            entity.HasOne(d => d.IdCajaNavigation).WithMany(p => p.RegistrosDeCajas)
                .HasForeignKey(d => d.IdCaja)
                .HasConstraintName("FK__Registros__id_ca__403A8C7D");

            entity.HasOne(d => d.IdOperacionNavigation).WithMany(p => p.RegistrosDeCajas)
                .HasForeignKey(d => d.IdOperacion)
                .HasConstraintName("FK__Registros__id_Op__3F466844");

            entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.RegistrosDeCajas)
                .HasForeignKey(d => d.IdTurno)
                .HasConstraintName("FK__Registros__id_tu__4222D4EF");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.RegistrosDeCajas)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Registros__id_ve__412EB0B6");
        });

        modelBuilder.Entity<Seguro>(entity =>
        {
            entity.HasKey(e => e.IdSeguro).HasName("PK__Seguros__D187EEFEACD72954");

            entity.Property(e => e.IdSeguro).HasColumnName("id_seguro");
            entity.Property(e => e.Aseguradora)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("aseguradora");
            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo_");
            entity.Property(e => e.NroPoliza)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nroPoliza");
            entity.Property(e => e.VigenciaDesde)
                .HasColumnType("date")
                .HasColumnName("vigenciaDesde");
            entity.Property(e => e.VigenciaHasta)
                .HasColumnType("date")
                .HasColumnName("vigenciaHasta");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Seguros)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__Seguros__id_vehi__45F365D3");
        });

        modelBuilder.Entity<TiposDeCaja>(entity =>
        {
            entity.HasKey(e => e.IdCaja).HasName("PK__TiposDeC__C71E2476F4AA21F0");

            entity.Property(e => e.IdCaja).HasColumnName("id_caja");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("((1))")
                .HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.NomCaja)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nomCaja");
        });

        modelBuilder.Entity<TiposDeOperacione>(entity =>
        {
            entity.HasKey(e => e.IdOperacion).HasName("PK__TiposDeO__20CCE2A07792461B");

            entity.Property(e => e.IdOperacion).HasColumnName("id_Operacion");
            entity.Property(e => e.NomOperacion)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nomOperacion");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.IdTurno).HasName("PK__Turnos__C68E7397F53557B3");

            entity.Property(e => e.IdTurno).HasColumnName("id_turno");
            entity.Property(e => e.HoraFin).HasColumnName("horaFin");
            entity.Property(e => e.HoraInicio).HasColumnName("horaInicio");
            entity.Property(e => e.NomTurno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nomTurno");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__F5DC0F39FEA22D64");

            entity.Property(e => e.IdVehiculo).HasColumnName("id_vehiculo");
            entity.Property(e => e.Activo)
                .HasDefaultValueSql("((1))")
                .HasColumnName("activo");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Chapa)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("chapa");
            entity.Property(e => e.Cilindraje)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cilindraje");
            entity.Property(e => e.Marca)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("marca");
            entity.Property(e => e.Modelo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("modelo");
            entity.Property(e => e.NroChasis)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("nroChasis");
            entity.Property(e => e.NroMotor)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("nroMotor");
            entity.Property(e => e.Patente)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("patente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
