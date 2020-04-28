namespace JewelryShop.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddOfficeAddressColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeAddres",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OfficeAddres",
                table: "ShippingAddresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeAddres",
                table: "ShippingAddresses");

            migrationBuilder.AddColumn<string>(
                name: "OfficeAddres",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
