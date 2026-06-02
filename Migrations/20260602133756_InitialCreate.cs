using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AKlasa.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Druzyna",
                columns: table => new
                {
                    IdDruzyny = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NazwaKlubu = table.Column<string>(type: "TEXT", nullable: false),
                    IloscMeczy = table.Column<int>(type: "INTEGER", nullable: false),
                    IloscZwyciestw = table.Column<int>(type: "INTEGER", nullable: false),
                    IloscRemisow = table.Column<int>(type: "INTEGER", nullable: false),
                    IloscPorazek = table.Column<int>(type: "INTEGER", nullable: false),
                    BramkiZdobyte = table.Column<int>(type: "INTEGER", nullable: false),
                    BramkiStracone = table.Column<int>(type: "INTEGER", nullable: false),
                    BilansBramkowy = table.Column<int>(type: "INTEGER", nullable: false),
                    Punkty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Druzyna", x => x.IdDruzyny);
                });

            migrationBuilder.CreateTable(
                name: "Mecz",
                columns: table => new
                {
                    IdMeczu = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GospodarzId = table.Column<int>(type: "INTEGER", nullable: true),
                    GoscId = table.Column<int>(type: "INTEGER", nullable: true),
                    BramkiGospodarzy = table.Column<int>(type: "INTEGER", nullable: false),
                    BramkiGosci = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mecz", x => x.IdMeczu);
                    table.ForeignKey(
                        name: "FK_Mecz_Druzyna_GoscId",
                        column: x => x.GoscId,
                        principalTable: "Druzyna",
                        principalColumn: "IdDruzyny",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mecz_Druzyna_GospodarzId",
                        column: x => x.GospodarzId,
                        principalTable: "Druzyna",
                        principalColumn: "IdDruzyny",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zawodnik",
                columns: table => new
                {
                    IdZawodnika = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    IloscBramek = table.Column<int>(type: "INTEGER", nullable: false),
                    IloscAsyst = table.Column<int>(type: "INTEGER", nullable: false),
                    DruzynaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zawodnik", x => x.IdZawodnika);
                    table.ForeignKey(
                        name: "FK_Zawodnik_Druzyna_DruzynaId",
                        column: x => x.DruzynaId,
                        principalTable: "Druzyna",
                        principalColumn: "IdDruzyny");
                });

            migrationBuilder.CreateTable(
                name: "Transfer",
                columns: table => new
                {
                    IdTransferu = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ZawodnikId = table.Column<int>(type: "INTEGER", nullable: true),
                    DruzynaOdId = table.Column<int>(type: "INTEGER", nullable: true),
                    DruzynaDoId = table.Column<int>(type: "INTEGER", nullable: true),
                    DataTransferu = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer", x => x.IdTransferu);
                    table.ForeignKey(
                        name: "FK_Transfer_Druzyna_DruzynaDoId",
                        column: x => x.DruzynaDoId,
                        principalTable: "Druzyna",
                        principalColumn: "IdDruzyny",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfer_Druzyna_DruzynaOdId",
                        column: x => x.DruzynaOdId,
                        principalTable: "Druzyna",
                        principalColumn: "IdDruzyny",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfer_Zawodnik_ZawodnikId",
                        column: x => x.ZawodnikId,
                        principalTable: "Zawodnik",
                        principalColumn: "IdZawodnika");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mecz_GoscId",
                table: "Mecz",
                column: "GoscId");

            migrationBuilder.CreateIndex(
                name: "IX_Mecz_GospodarzId",
                table: "Mecz",
                column: "GospodarzId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_DruzynaDoId",
                table: "Transfer",
                column: "DruzynaDoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_DruzynaOdId",
                table: "Transfer",
                column: "DruzynaOdId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_ZawodnikId",
                table: "Transfer",
                column: "ZawodnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Zawodnik_DruzynaId",
                table: "Zawodnik",
                column: "DruzynaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mecz");

            migrationBuilder.DropTable(
                name: "Transfer");

            migrationBuilder.DropTable(
                name: "Zawodnik");

            migrationBuilder.DropTable(
                name: "Druzyna");
        }
    }
}
