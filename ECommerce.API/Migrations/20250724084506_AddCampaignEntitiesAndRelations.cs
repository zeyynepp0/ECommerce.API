using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignEntitiesAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "MinAmount",
                table: "Campaigns",
                newName: "Percentage");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Campaigns",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuyQuantity",
                table: "Campaigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderAmount",
                table: "Campaigns",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayQuantity",
                table: "Campaigns",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "BuyQuantity",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "MinOrderAmount",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "PayQuantity",
                table: "Campaigns");

            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "Campaigns",
                newName: "MinAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "Campaigns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
