using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderForShippingCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCompany",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShippingCompanyId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingCompanyId",
                table: "Orders",
                column: "ShippingCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingCompanies_ShippingCompanyId",
                table: "Orders",
                column: "ShippingCompanyId",
                principalTable: "ShippingCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingCompanies_ShippingCompanyId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingCompanyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCompanyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ShippingCompany",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
