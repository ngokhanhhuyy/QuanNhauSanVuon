using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanNhauSanVuon.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMenuItemDefaultAmountToDefaultNetPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "default_amount",
                table: "menu_items",
                newName: "default_net_price");

            migrationBuilder.AddColumn<long>(
                name: "net_price_per_unit",
                table: "order_items",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "vat_amount_per_unit",
                table: "order_items",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "net_price_per_unit",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "vat_amount_per_unit",
                table: "order_items");

            migrationBuilder.RenameColumn(
                name: "default_net_price",
                table: "menu_items",
                newName: "default_amount");
        }
    }
}
