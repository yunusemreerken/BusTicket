using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicket.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voyages_Courses_CourseId",
                table: "Voyages");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.CreateTable(
                name: "BusRoutes",
                columns: table => new
                {
                    BusRouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusRoutes", x => x.BusRouteId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Voyages_BusRoutes_CourseId",
                table: "Voyages",
                column: "CourseId",
                principalTable: "BusRoutes",
                principalColumn: "BusRouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voyages_BusRoutes_CourseId",
                table: "Voyages");

            migrationBuilder.DropTable(
                name: "BusRoutes");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivalCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TakeOffCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Voyages_Courses_CourseId",
                table: "Voyages",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }
    }
}
