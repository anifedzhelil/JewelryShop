using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JewelryShop.Data.Migrations
{
    public partial class AddJewelImagesModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JewelryImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    JewelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JewelryImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JewelryImages_Jewelry_JewelId",
                        column: x => x.JewelId,
                        principalTable: "Jewelry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JewelryImages_JewelId",
                table: "JewelryImages",
                column: "JewelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JewelryImages");
        }
    }
}
