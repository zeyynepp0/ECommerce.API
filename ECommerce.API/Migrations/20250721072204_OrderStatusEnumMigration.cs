using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.API.Migrations
{
    /// <inheritdoc />
    public partial class OrderStatusEnumMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Enum karşılıkları:
            // 'pending'   -> 0
            // 'approved'  -> 1
            // 'preparing' -> 2
            // 'shipped'   -> 3
            // 'delivered' -> 4
            // 'cancelled' -> 5
            // 'returned'  -> 6
            // 'refunded'  -> 7
            migrationBuilder.Sql(@"
                UPDATE Orders SET Status = '0' WHERE Status = 'pending';
                UPDATE Orders SET Status = '1' WHERE Status = 'approved';
                UPDATE Orders SET Status = '2' WHERE Status = 'preparing';
                UPDATE Orders SET Status = '3' WHERE Status = 'shipped';
                UPDATE Orders SET Status = '4' WHERE Status = 'delivered';
                UPDATE Orders SET Status = '5' WHERE Status = 'cancelled';
                UPDATE Orders SET Status = '6' WHERE Status = 'returned';
                UPDATE Orders SET Status = '7' WHERE Status = 'refunded';
            ");
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
