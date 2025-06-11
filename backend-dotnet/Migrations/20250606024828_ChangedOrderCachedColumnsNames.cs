using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanNhauSanVuon.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOrderCachedColumnsNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "vat_amount",
                table: "orders",
                newName: "cached_vat_amount");

            migrationBuilder.RenameColumn(
                name: "item_amount",
                table: "orders",
                newName: "cached_net_amount");

            migrationBuilder.AddColumn<long>(
                name: "cached_gross_amount",
                table: "orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cached_gross_amount",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "cached_vat_amount",
                table: "orders",
                newName: "vat_amount");

            migrationBuilder.RenameColumn(
                name: "cached_net_amount",
                table: "orders",
                newName: "item_amount");
        }
    }
}
