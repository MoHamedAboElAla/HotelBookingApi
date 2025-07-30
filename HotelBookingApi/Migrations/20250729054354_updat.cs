using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingApi.Migrations
{
    /// <inheritdoc />
    public partial class updat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Season_SeasonId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceFactor",
                table: "Seasons",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Bookings",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Seasons_SeasonId",
                table: "Bookings",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Seasons_SeasonId",
                table: "Bookings");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceFactor",
                table: "Seasons",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "int", nullable: true),
                    TempId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_Season_TempId", x => x.TempId);
                    table.ForeignKey(
                        name: "FK_Season_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id");
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Season_SeasonId",
                table: "Bookings",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "TempId");
        }
    }
}
