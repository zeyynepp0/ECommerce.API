using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignCategory_Campaign_CampaignId",
                table: "CampaignCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignCategory_Categories_CategoryId",
                table: "CampaignCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignProduct_Campaign_CampaignId",
                table: "CampaignProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignProduct_Products_ProductId",
                table: "CampaignProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignProduct",
                table: "CampaignProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignCategory",
                table: "CampaignCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaign",
                table: "Campaign");

            migrationBuilder.RenameTable(
                name: "CampaignProduct",
                newName: "CampaignProducts");

            migrationBuilder.RenameTable(
                name: "CampaignCategory",
                newName: "CampaignCategories");

            migrationBuilder.RenameTable(
                name: "Campaign",
                newName: "Campaigns");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignProduct_ProductId",
                table: "CampaignProducts",
                newName: "IX_CampaignProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignProduct_CampaignId",
                table: "CampaignProducts",
                newName: "IX_CampaignProducts_CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignCategory_CategoryId",
                table: "CampaignCategories",
                newName: "IX_CampaignCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignCategory_CampaignId",
                table: "CampaignCategories",
                newName: "IX_CampaignCategories_CampaignId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignProducts",
                table: "CampaignProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignCategories",
                table: "CampaignCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignCategories_Campaigns_CampaignId",
                table: "CampaignCategories",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignCategories_Categories_CategoryId",
                table: "CampaignCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignProducts_Campaigns_CampaignId",
                table: "CampaignProducts",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignProducts_Products_ProductId",
                table: "CampaignProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignCategories_Campaigns_CampaignId",
                table: "CampaignCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignCategories_Categories_CategoryId",
                table: "CampaignCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignProducts_Campaigns_CampaignId",
                table: "CampaignProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignProducts_Products_ProductId",
                table: "CampaignProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignProducts",
                table: "CampaignProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignCategories",
                table: "CampaignCategories");

            migrationBuilder.RenameTable(
                name: "Campaigns",
                newName: "Campaign");

            migrationBuilder.RenameTable(
                name: "CampaignProducts",
                newName: "CampaignProduct");

            migrationBuilder.RenameTable(
                name: "CampaignCategories",
                newName: "CampaignCategory");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignProducts_ProductId",
                table: "CampaignProduct",
                newName: "IX_CampaignProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignProducts_CampaignId",
                table: "CampaignProduct",
                newName: "IX_CampaignProduct_CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignCategories_CategoryId",
                table: "CampaignCategory",
                newName: "IX_CampaignCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignCategories_CampaignId",
                table: "CampaignCategory",
                newName: "IX_CampaignCategory_CampaignId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaign",
                table: "Campaign",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignProduct",
                table: "CampaignProduct",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignCategory",
                table: "CampaignCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignCategory_Campaign_CampaignId",
                table: "CampaignCategory",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignCategory_Categories_CategoryId",
                table: "CampaignCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignProduct_Campaign_CampaignId",
                table: "CampaignProduct",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignProduct_Products_ProductId",
                table: "CampaignProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
