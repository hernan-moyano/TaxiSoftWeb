using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiSoftWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposAlquilerConductore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "deposito_garantia",
                table: "Conductores",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_ingreso",
                table: "Conductores",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "monto_alquiler",
                table: "Conductores",
                type: "numeric(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "Conductores",
                type: "character varying(300)",
                unicode: false,
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "porcentaje_recaudacion",
                table: "Conductores",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tipo_alquiler",
                table: "Conductores",
                type: "character varying(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deposito_garantia",
                table: "Conductores");

            migrationBuilder.DropColumn(
                name: "fecha_ingreso",
                table: "Conductores");

            migrationBuilder.DropColumn(
                name: "monto_alquiler",
                table: "Conductores");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "Conductores");

            migrationBuilder.DropColumn(
                name: "porcentaje_recaudacion",
                table: "Conductores");

            migrationBuilder.DropColumn(
                name: "tipo_alquiler",
                table: "Conductores");
        }
    }
}
