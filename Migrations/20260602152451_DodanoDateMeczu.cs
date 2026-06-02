using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AKlasa.Migrations
{
    /// <inheritdoc />
    public partial class DodanoDateMeczu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataMeczu",
                table: "Mecz",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataMeczu",
                table: "Mecz");
        }
    }
}
