using Microsoft.EntityFrameworkCore.Migrations;

namespace JewelryShop.Data.Migrations
{
    public partial class GuestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuestId",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Orders");
        }
    }
}
