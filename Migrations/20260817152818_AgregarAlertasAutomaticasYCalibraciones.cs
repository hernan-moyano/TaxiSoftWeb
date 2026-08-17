using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaxiSoftWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarAlertasAutomaticasYCalibraciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertasAutomaticas",
                columns: table => new
                {
                    id_alerta_auto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true),
                    cuil_ = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: true),
                    tipo_documento = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    id_registro_origen = table.Column<int>(type: "integer", nullable: true),
                    fecha_vencimiento = table.Column<DateTime>(type: "date", nullable: true),
                    descripcion = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    dias_antelacion = table.Column<int>(type: "integer", nullable: true),
                    activa = table.Column<bool>(type: "boolean", nullable: true, defaultValueSql: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AlertaAutomatica", x => x.id_alerta_auto);
                    table.ForeignKey(
                        name: "FK__AlertasAuto__cuil",
                        column: x => x.cuil_,
                        principalTable: "Conductores",
                        principalColumn: "cuil");
                    table.ForeignKey(
                        name: "FK__AlertasAuto__id_vehiculo",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateTable(
                name: "CalibracionesTaximetro",
                columns: table => new
                {
                    id_calibracion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true),
                    fecha_calibracion = table.Column<DateTime>(type: "date", nullable: true),
                    proxima_calibracion = table.Column<DateTime>(type: "date", nullable: true),
                    entidad_calibradora = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    nro_certificado = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    valor = table.Column<decimal>(type: "numeric(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CalibracionTaximetro", x => x.id_calibracion);
                    table.ForeignKey(
                        name: "FK__Calibracion__id_vehiculo",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlertasAutomaticas_cuil_",
                table: "AlertasAutomaticas",
                column: "cuil_");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasAutomaticas_id_vehiculo_",
                table: "AlertasAutomaticas",
                column: "id_vehiculo_");

            migrationBuilder.CreateIndex(
                name: "IX_CalibracionesTaximetro_id_vehiculo_",
                table: "CalibracionesTaximetro",
                column: "id_vehiculo_");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasAutomaticas");

            migrationBuilder.DropTable(
                name: "CalibracionesTaximetro");
        }
    }
}
