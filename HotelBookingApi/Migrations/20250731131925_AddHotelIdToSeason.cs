using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelIdToSeason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Seasons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_HotelId",
                table: "Seasons",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Hotels_HotelId",
                table: "Seasons",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Hotels_HotelId",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_HotelId",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Seasons");
        }
    }
}
