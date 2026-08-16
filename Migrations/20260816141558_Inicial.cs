using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaxiSoftWeb.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carnets",
                columns: table => new
                {
                    id_carnet = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nroCarnet = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true),
                    vtoCarnet = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Carnets__1688C19283540861", x => x.id_carnet);
                });

            migrationBuilder.CreateTable(
                name: "Domicilios",
                columns: table => new
                {
                    id_domicilio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    calle = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: true),
                    numero = table.Column<int>(type: "integer", nullable: true),
                    piso = table.Column<string>(type: "character varying(4)", unicode: false, maxLength: 4, nullable: true),
                    departamento = table.Column<string>(type: "character varying(15)", unicode: false, maxLength: 15, nullable: true),
                    ciudad = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Domicili__A0CCE5C2CB275F17", x => x.id_domicilio);
                });

            migrationBuilder.CreateTable(
                name: "EstadosActividades",
                columns: table => new
                {
                    Id_estadoA = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomEstadoA = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadosA__6B32751C2CDF43A2", x => x.Id_estadoA);
                });

            migrationBuilder.CreateTable(
                name: "EstadosPagos",
                columns: table => new
                {
                    Id_estadoP = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomEstadoP = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadosP__6B32750D8A15EC4E", x => x.Id_estadoP);
                });

            migrationBuilder.CreateTable(
                name: "Puestos",
                columns: table => new
                {
                    id_puesto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    califProfesional = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true),
                    tarDesepeniada = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Puestos__123AAB99807408D8", x => x.id_puesto);
                });

            migrationBuilder.CreateTable(
                name: "TiposDeCajas",
                columns: table => new
                {
                    id_caja = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomCaja = table.Column<string>(type: "character varying(25)", unicode: false, maxLength: 25, nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: true, defaultValueSql: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TiposDeC__C71E2476F4AA21F0", x => x.id_caja);
                });

            migrationBuilder.CreateTable(
                name: "TiposDeOperaciones",
                columns: table => new
                {
                    id_Operacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomOperacion = table.Column<string>(type: "character varying(25)", unicode: false, maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TiposDeO__20CCE2A07792461B", x => x.id_Operacion);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    id_turno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomTurno = table.Column<string>(type: "character varying(25)", unicode: false, maxLength: 25, nullable: true),
                    horaInicio = table.Column<TimeSpan>(type: "interval", nullable: true),
                    horaFin = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Turnos__C68E7397F53557B3", x => x.id_turno);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    id_vehiculo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    patente = table.Column<string>(type: "character varying(7)", unicode: false, maxLength: 7, nullable: true),
                    chapa = table.Column<string>(type: "character varying(7)", unicode: false, maxLength: 7, nullable: true),
                    marca = table.Column<string>(type: "character varying(15)", unicode: false, maxLength: 15, nullable: true),
                    modelo = table.Column<string>(type: "character varying(15)", unicode: false, maxLength: 15, nullable: true),
                    anio = table.Column<int>(type: "integer", nullable: true),
                    cilindraje = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    nroMotor = table.Column<string>(type: "character varying(17)", unicode: false, maxLength: 17, nullable: true),
                    nroChasis = table.Column<string>(type: "character varying(17)", unicode: false, maxLength: 17, nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: true, defaultValueSql: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Vehiculo__F5DC0F39FEA22D64", x => x.id_vehiculo);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    id_alerta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaDesde = table.Column<DateTime>(type: "timestamp", nullable: true),
                    fechaHasta = table.Column<DateTime>(type: "timestamp", nullable: true),
                    diasAnticipacion = table.Column<int>(type: "integer", nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    Id_estadoA_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Alertas__1227953E3A482B74", x => x.id_alerta);
                    table.ForeignKey(
                        name: "FK__Alertas__Id_esta__571DF1D5",
                        column: x => x.Id_estadoA_,
                        principalTable: "EstadosActividades",
                        principalColumn: "Id_estadoA");
                });

            migrationBuilder.CreateTable(
                name: "Conductores",
                columns: table => new
                {
                    cuil = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: false),
                    dni = table.Column<string>(type: "character varying(8)", unicode: false, maxLength: 8, nullable: true),
                    apellido = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true),
                    nombre = table.Column<string>(type: "character varying(70)", unicode: false, maxLength: 70, nullable: true),
                    fechaNacimiento = table.Column<DateTime>(type: "date", nullable: true),
                    telefono = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true),
                    id_domicilio_ = table.Column<int>(type: "integer", nullable: true),
                    id_carnet_ = table.Column<int>(type: "integer", nullable: true),
                    id_puesto_ = table.Column<int>(type: "integer", nullable: true),
                    id_turno_ = table.Column<int>(type: "integer", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: true, defaultValueSql: "true"),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Conducto__2CDD98AE290E0992", x => x.cuil);
                    table.ForeignKey(
                        name: "FK__Conductor__id_ca__3B75D760",
                        column: x => x.id_carnet_,
                        principalTable: "Carnets",
                        principalColumn: "id_carnet");
                    table.ForeignKey(
                        name: "FK__Conductor__id_do__3C69FB99",
                        column: x => x.id_domicilio_,
                        principalTable: "Domicilios",
                        principalColumn: "id_domicilio");
                    table.ForeignKey(
                        name: "FK__Conductor__id_pu__3A81B327",
                        column: x => x.id_puesto_,
                        principalTable: "Puestos",
                        principalColumn: "id_puesto");
                    table.ForeignKey(
                        name: "FK__Conductor__id_tu__398D8EEE",
                        column: x => x.id_turno_,
                        principalTable: "Turnos",
                        principalColumn: "id_turno");
                    table.ForeignKey(
                        name: "FK__Conductor__id_ve__38996AB5",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateTable(
                name: "Impuestos",
                columns: table => new
                {
                    id_impuesto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaVto = table.Column<DateTime>(type: "date", nullable: true),
                    valor = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true),
                    Id_estadoP_ = table.Column<int>(type: "integer", nullable: true),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Impuesto__8546BDFCFB2927D6", x => x.id_impuesto);
                    table.ForeignKey(
                        name: "FK__Impuestos__Id_es__5070F446",
                        column: x => x.Id_estadoP_,
                        principalTable: "EstadosPagos",
                        principalColumn: "Id_estadoP");
                    table.ForeignKey(
                        name: "FK__Impuestos__id_ve__4F7CD00D",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateTable(
                name: "ITVs",
                columns: table => new
                {
                    id_itv = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vigenciaDesde = table.Column<DateTime>(type: "date", nullable: true),
                    vigenciaHasta = table.Column<DateTime>(type: "date", nullable: true),
                    descripcion = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    valor = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ITVs__D62ADFEEB7B3E2D0", x => x.id_itv);
                    table.ForeignKey(
                        name: "FK__ITVs__id_vehicul__48CFD27E",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateTable(
                name: "Mantenimientos",
                columns: table => new
                {
                    id_mant = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaMant = table.Column<DateTime>(type: "timestamp", nullable: true),
                    descripcion = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    valor = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true),
                    Id_estadoA_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Mantenim__6FA198DEA4AFF90B", x => x.id_mant);
                    table.ForeignKey(
                        name: "FK__Mantenimi__Id_es__534D60F1",
                        column: x => x.Id_estadoA_,
                        principalTable: "EstadosActividades",
                        principalColumn: "Id_estadoA");
                    table.ForeignKey(
                        name: "FK__Mantenimi__id_ve__5441852A",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateTable(
                name: "Multas",
                columns: table => new
                {
                    id_multa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaVto = table.Column<DateTime>(type: "date", nullable: true),
                    descripcion = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    valor = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true),
                    Id_estadoP_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Multas__295650BBD8300E64", x => x.id_multa);
                    table.ForeignKey(
                        name: "FK__Multas__Id_estad__4BAC3F29",
                        column: x => x.Id_estadoP_,
                        principalTable: "EstadosPagos",
                        principalColumn: "Id_estadoP");
                    table.ForeignKey(
                        name: "FK__Multas__id_vehic__4CA06362",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateTable(
                name: "Seguros",
                columns: table => new
                {
                    id_seguro = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nroPoliza = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    aseguradora = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true),
                    vigenciaDesde = table.Column<DateTime>(type: "date", nullable: true),
                    vigenciaHasta = table.Column<DateTime>(type: "date", nullable: true),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seguros__D187EEFEACD72954", x => x.id_seguro);
                    table.ForeignKey(
                        name: "FK__Seguros__id_vehi__45F365D3",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateTable(
                name: "RegistrosDeCajas",
                columns: table => new
                {
                    id_registroCaja = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaRegisCaja = table.Column<DateTime>(type: "timestamp", nullable: true),
                    concepto = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: true),
                    importe = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    id_turno_ = table.Column<int>(type: "integer", nullable: true),
                    cuil_ = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: true),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true),
                    id_caja_ = table.Column<int>(type: "integer", nullable: true),
                    id_Operacion_ = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Registro__53E7860F39F615BE", x => x.id_registroCaja);
                    table.ForeignKey(
                        name: "FK__Registros__cuil___4316F928",
                        column: x => x.cuil_,
                        principalTable: "Conductores",
                        principalColumn: "cuil");
                    table.ForeignKey(
                        name: "FK__Registros__id_Op__3F466844",
                        column: x => x.id_Operacion_,
                        principalTable: "TiposDeOperaciones",
                        principalColumn: "id_Operacion");
                    table.ForeignKey(
                        name: "FK__Registros__id_ca__403A8C7D",
                        column: x => x.id_caja_,
                        principalTable: "TiposDeCajas",
                        principalColumn: "id_caja");
                    table.ForeignKey(
                        name: "FK__Registros__id_tu__4222D4EF",
                        column: x => x.id_turno_,
                        principalTable: "Turnos",
                        principalColumn: "id_turno");
                    table.ForeignKey(
                        name: "FK__Registros__id_ve__412EB0B6",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Id_estadoA_",
                table: "Alertas",
                column: "Id_estadoA_");

            migrationBuilder.CreateIndex(
                name: "IX_Conductores_id_carnet_",
                table: "Conductores",
                column: "id_carnet_");

            migrationBuilder.CreateIndex(
                name: "IX_Conductores_id_domicilio_",
                table: "Conductores",
                column: "id_domicilio_");

            migrationBuilder.CreateIndex(
                name: "IX_Conductores_id_puesto_",
                table: "Conductores",
                column: "id_puesto_");

            migrationBuilder.CreateIndex(
                name: "IX_Conductores_id_turno_",
                table: "Conductores",
                column: "id_turno_");

            migrationBuilder.CreateIndex(
                name: "IX_Conductores_id_vehiculo_",
                table: "Conductores",
                column: "id_vehiculo_");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_Id_estadoP_",
                table: "Impuestos",
                column: "Id_estadoP_");

            migrationBuilder.CreateIndex(
                name: "IX_Impuestos_id_vehiculo_",
                table: "Impuestos",
                column: "id_vehiculo_");

            migrationBuilder.CreateIndex(
                name: "IX_ITVs_id_vehiculo_",
                table: "ITVs",
                column: "id_vehiculo_");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_Id_estadoA_",
                table: "Mantenimientos",
                column: "Id_estadoA_");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimientos_id_vehiculo_",
                table: "Mantenimientos",
                column: "id_vehiculo_");

            migrationBuilder.CreateIndex(
                name: "IX_Multas_Id_estadoP_",
                table: "Multas",
                column: "Id_estadoP_");

            migrationBuilder.CreateIndex(
                name: "IX_Multas_id_vehiculo_",
                table: "Multas",
                column: "id_vehiculo_");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeCajas_cuil_",
                table: "RegistrosDeCajas",
                column: "cuil_");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeCajas_id_caja_",
                table: "RegistrosDeCajas",
                column: "id_caja_");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeCajas_id_Operacion_",
                table: "RegistrosDeCajas",
                column: "id_Operacion_");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeCajas_id_turno_",
                table: "RegistrosDeCajas",
                column: "id_turno_");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosDeCajas_id_vehiculo_",
                table: "RegistrosDeCajas",
                column: "id_vehiculo_");

            migrationBuilder.CreateIndex(
                name: "IX_Seguros_id_vehiculo_",
                table: "Seguros",
                column: "id_vehiculo_");

            migrationBuilder.Sql(@"INSERT INTO ""EstadosActividades"" (""Id_estadoA"", ""nomEstadoA"") VALUES
(1, 'Activo'), (2, 'Inactivo'), (3, 'Pendiente'), (4, 'Finalizado');");

            migrationBuilder.Sql(@"INSERT INTO ""EstadosPagos"" (""Id_estadoP"", ""nomEstadoP"") VALUES
(1, 'Pagado'), (2, 'Pendiente'), (3, 'Vencido');");

            migrationBuilder.Sql(@"INSERT INTO ""TiposDeCajas"" (""id_caja"", ""nomCaja"", ""Descripcion"", ""activo"") VALUES
(1, 'Ingreso', 'Ingresos de caja', true),
(2, 'Egreso', 'Egresos de caja', true);");

            migrationBuilder.Sql(@"INSERT INTO ""TiposDeOperaciones"" (""id_Operacion"", ""nomOperacion"") VALUES
(1, 'Viaje'), (2, 'Mantenimiento'), (3, 'Multa'), (4, 'Impuesto'), (5, 'Seguro'), (6, 'ITV');");

            migrationBuilder.Sql(@"INSERT INTO ""Turnos"" (""id_turno"", ""nomTurno"", ""horaInicio"", ""horaFin"") VALUES
(1, 'Mañana', '06:00:00', '14:00:00'),
(2, 'Tarde', '14:00:00', '22:00:00'),
(3, 'Noche', '22:00:00', '06:00:00');");

            migrationBuilder.Sql(@"INSERT INTO ""Puestos"" (""id_puesto"", ""califProfesional"", ""tarDesepeniada"") VALUES
(1, 'Chofer', 'Conductor de taxi'),
(2, 'Titular', 'Titular de licencia');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Impuestos");

            migrationBuilder.DropTable(
                name: "ITVs");

            migrationBuilder.DropTable(
                name: "Mantenimientos");

            migrationBuilder.DropTable(
                name: "Multas");

            migrationBuilder.DropTable(
                name: "RegistrosDeCajas");

            migrationBuilder.DropTable(
                name: "Seguros");

            migrationBuilder.DropTable(
                name: "EstadosActividades");

            migrationBuilder.DropTable(
                name: "EstadosPagos");

            migrationBuilder.DropTable(
                name: "Conductores");

            migrationBuilder.DropTable(
                name: "TiposDeOperaciones");

            migrationBuilder.DropTable(
                name: "TiposDeCajas");

            migrationBuilder.DropTable(
                name: "Carnets");

            migrationBuilder.DropTable(
                name: "Domicilios");

            migrationBuilder.DropTable(
                name: "Puestos");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Vehiculos");
        }
    }
}
