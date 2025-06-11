using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuanNhauSanVuon.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionColumnToOrderOrderItemAndUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_items_users_created_user_id",
                table: "order_items");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_users_created_user_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_order_items_created_user_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "created_user_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "last_updated_datetime",
                table: "order_items");

            migrationBuilder.AddColumn<int>(
                name: "last_updated_user_id",
                table: "orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "row_version",
                table: "orders",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "row_version",
                table: "order_items",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_last_updated_user_id",
                table: "orders",
                column: "last_updated_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk__orders__users__created_user_id",
                table: "orders",
                column: "created_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk__orders__users__last_updated_user_id",
                table: "orders",
                column: "last_updated_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk__orders__users__created_user_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk__orders__users__last_updated_user_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_last_updated_user_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "last_updated_user_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "row_version",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "row_version",
                table: "order_items");

            migrationBuilder.AddColumn<int>(
                name: "created_user_id",
                table: "order_items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_updated_datetime",
                table: "order_items",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_items_created_user_id",
                table: "order_items",
                column: "created_user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_order_items_users_created_user_id",
                table: "order_items",
                column: "created_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_users_created_user_id",
                table: "orders",
                column: "created_user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
