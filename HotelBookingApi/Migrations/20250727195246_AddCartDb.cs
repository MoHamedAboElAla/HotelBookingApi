using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBookingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCartDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Seasons",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_HotelId",
                table: "Seasons",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_AgentId",
                table: "CartItems",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_BookingId",
                table: "CartItems",
                column: "BookingId");

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

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_HotelId",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Seasons");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Bookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
