using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiSoftWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFrecuenciaAlertas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "frecuencia",
                table: "Alertas",
                type: "character varying(20)",
                unicode: false,
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "frecuencia",
                table: "Alertas");
        }
    }
}
