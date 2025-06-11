using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanNhauSanVuon.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeatingAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "unique__menu_category__name",
                table: "menu_categories",
                newName: "unique__menu_categories__name");

            migrationBuilder.AddColumn<int>(
                name: "area_id",
                table: "seatings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "seating_areas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    color = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    taken_up_positions_json = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seating_areas", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_seatings_area_id",
                table: "seatings",
                column: "area_id");

            migrationBuilder.AddForeignKey(
                name: "fk__seatings__seating_areas__area_id",
                table: "seatings",
                column: "area_id",
                principalTable: "seating_areas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk__seatings__seating_areas__area_id",
                table: "seatings");

            migrationBuilder.DropTable(
                name: "seating_areas");

            migrationBuilder.DropIndex(
                name: "IX_seatings_area_id",
                table: "seatings");

            migrationBuilder.DropColumn(
                name: "area_id",
                table: "seatings");

            migrationBuilder.RenameIndex(
                name: "unique__menu_categories__name",
                table: "menu_categories",
                newName: "unique__menu_category__name");
        }
    }
}
