using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanNhauSanVuon.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeatingAreaTableAndChangedTableTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "unique__seatings__name",
                table: "seatings",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "unique__seating_areas__name",
                table: "seating_areas",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "unique__seatings__name",
                table: "seatings");

            migrationBuilder.DropIndex(
                name: "unique__seating_areas__name",
                table: "seating_areas");
        }
    }
}
