using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reservas.BData.Migrations
{
    /// <inheritdoc />
    public partial class CambiosNuevosAgregados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Reservas");

            migrationBuilder.AlterColumn<string>(
                name: "Fecha_inicio",
                table: "Reservas",
                type: "char(10)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "Fecha_fin",
                table: "Reservas",
                type: "char(10)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha_inicio",
                table: "Reservas",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(10)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha_fin",
                table: "Reservas",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(10)");

            migrationBuilder.AddColumn<string>(
                name: "DateTime",
                table: "Reservas",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");
        }
    }
}
