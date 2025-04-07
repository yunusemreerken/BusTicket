using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicket.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guzergahlar",
                columns: table => new
                {
                    GuzergahID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KalkisSehri = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VarisSehri = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guzergahlar", x => x.GuzergahID);
                });

            migrationBuilder.CreateTable(
                name: "Otobusler",
                columns: table => new
                {
                    OtobusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plaka = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    KoltukKapasitesi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otobusler", x => x.OtobusID);
                });

            migrationBuilder.CreateTable(
                name: "Yolcular",
                columns: table => new
                {
                    YolcuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yolcular", x => x.YolcuID);
                });

            migrationBuilder.CreateTable(
                name: "Seferler",
                columns: table => new
                {
                    SeferID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuzergahID = table.Column<int>(type: "int", nullable: false),
                    OtobusID = table.Column<int>(type: "int", nullable: false),
                    KalkisZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VarisZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BiletFiyati = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seferler", x => x.SeferID);
                    table.ForeignKey(
                        name: "FK_Seferler_Guzergahlar_GuzergahID",
                        column: x => x.GuzergahID,
                        principalTable: "Guzergahlar",
                        principalColumn: "GuzergahID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seferler_Otobusler_OtobusID",
                        column: x => x.OtobusID,
                        principalTable: "Otobusler",
                        principalColumn: "OtobusID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Biletler",
                columns: table => new
                {
                    BiletID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeferID = table.Column<int>(type: "int", nullable: false),
                    KoltukNumarasi = table.Column<int>(type: "int", nullable: false),
                    YolcuID = table.Column<int>(type: "int", nullable: false),
                    SatinAlmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PNR = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biletler", x => x.BiletID);
                    table.ForeignKey(
                        name: "FK_Biletler_Seferler_SeferID",
                        column: x => x.SeferID,
                        principalTable: "Seferler",
                        principalColumn: "SeferID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Biletler_Yolcular_YolcuID",
                        column: x => x.YolcuID,
                        principalTable: "Yolcular",
                        principalColumn: "YolcuID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Koltuklar",
                columns: table => new
                {
                    KoltukID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeferID = table.Column<int>(type: "int", nullable: false),
                    KoltukNumarasi = table.Column<int>(type: "int", nullable: false),
                    DoluMu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koltuklar", x => x.KoltukID);
                    table.ForeignKey(
                        name: "FK_Koltuklar_Seferler_SeferID",
                        column: x => x.SeferID,
                        principalTable: "Seferler",
                        principalColumn: "SeferID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_PNR",
                table: "Biletler",
                column: "PNR",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_SeferID",
                table: "Biletler",
                column: "SeferID");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_YolcuID",
                table: "Biletler",
                column: "YolcuID");

            migrationBuilder.CreateIndex(
                name: "IX_Koltuklar_SeferID_KoltukNumarasi",
                table: "Koltuklar",
                columns: new[] { "SeferID", "KoltukNumarasi" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seferler_GuzergahID",
                table: "Seferler",
                column: "GuzergahID");

            migrationBuilder.CreateIndex(
                name: "IX_Seferler_OtobusID",
                table: "Seferler",
                column: "OtobusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biletler");

            migrationBuilder.DropTable(
                name: "Koltuklar");

            migrationBuilder.DropTable(
                name: "Yolcular");

            migrationBuilder.DropTable(
                name: "Seferler");

            migrationBuilder.DropTable(
                name: "Guzergahlar");

            migrationBuilder.DropTable(
                name: "Otobusler");
        }
    }
}
