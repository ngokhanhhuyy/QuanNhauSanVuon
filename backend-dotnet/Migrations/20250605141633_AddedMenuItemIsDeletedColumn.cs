using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanNhauSanVuon.Migrations
{
    /// <inheritdoc />
    public partial class AddedMenuItemIsDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "menu_items",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "menu_items");
        }
    }
}
