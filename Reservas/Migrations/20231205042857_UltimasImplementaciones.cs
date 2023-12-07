using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservas.BData.Migrations
{
    /// <inheritdoc />
    public partial class UltimasImplementaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EstadisponibleUoCupada",
                table: "Reservas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadisponibleUoCupada",
                table: "Reservas");
        }
    }
}
