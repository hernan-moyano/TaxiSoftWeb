using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaxiSoftWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarGastosOperacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposDeGastos",
                columns: table => new
                {
                    id_tipo_gasto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(30)", unicode: false, maxLength: 30, nullable: true),
                    descripcion = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: true, defaultValueSql: "true")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TipoGasto", x => x.id_tipo_gasto);
                });

            migrationBuilder.CreateTable(
                name: "GastosOperaciones",
                columns: table => new
                {
                    id_gasto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_vehiculo_ = table.Column<int>(type: "integer", nullable: true),
                    cuil_ = table.Column<string>(type: "character varying(11)", unicode: false, maxLength: 11, nullable: true),
                    fecha_gasto = table.Column<DateTime>(type: "date", nullable: true),
                    id_tipo_gasto = table.Column<int>(type: "integer", nullable: true),
                    descripcion = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    importe = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    kilometraje = table.Column<int>(type: "integer", nullable: true),
                    nro_factura = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GastoOperacion", x => x.id_gasto);
                    table.ForeignKey(
                        name: "FK__GastosOper__cuil",
                        column: x => x.cuil_,
                        principalTable: "Conductores",
                        principalColumn: "cuil");
                    table.ForeignKey(
                        name: "FK__GastosOper__id_tipo_gasto",
                        column: x => x.id_tipo_gasto,
                        principalTable: "TiposDeGastos",
                        principalColumn: "id_tipo_gasto");
                    table.ForeignKey(
                        name: "FK__GastosOper__id_vehiculo",
                        column: x => x.id_vehiculo_,
                        principalTable: "Vehiculos",
                        principalColumn: "id_vehiculo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GastosOperaciones_cuil_",
                table: "GastosOperaciones",
                column: "cuil_");

            migrationBuilder.CreateIndex(
                name: "IX_GastosOperaciones_id_tipo_gasto",
                table: "GastosOperaciones",
                column: "id_tipo_gasto");

            migrationBuilder.CreateIndex(
                name: "IX_GastosOperaciones_id_vehiculo_",
                table: "GastosOperaciones",
                column: "id_vehiculo_");

            migrationBuilder.InsertData(
                table: "TiposDeGastos",
                columns: new[] { "id_tipo_gasto", "nombre", "descripcion", "activo" },
                values: new object[,]
                {
                    { 1, "Combustible", "Consumo de combustible", true },
                    { 2, "Lavado", "Lavado y limpieza del vehículo", true },
                    { 3, "Neumáticos", "Compra y rotación de neumáticos", true },
                    { 4, "Reparación", "Reparaciones y mano de obra", true },
                    { 5, "Repuesto", "Compra de repuestos", true },
                    { 6, "Service", "Service técnico programado", true },
                    { 7, "Imprevisto mecánico", "Averías e imprevistos mecánicos", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GastosOperaciones");

            migrationBuilder.DropTable(
                name: "TiposDeGastos");
        }
    }
}
