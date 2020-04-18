namespace JewelryShop.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class JewelryRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JewelryRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    JewelId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Review = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JewelryRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JewelryRatings_Jewelry_JewelId",
                        column: x => x.JewelId,
                        principalTable: "Jewelry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JewelryRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JewelryRatings_JewelId",
                table: "JewelryRatings",
                column: "JewelId");

            migrationBuilder.CreateIndex(
                name: "IX_JewelryRatings_UserId",
                table: "JewelryRatings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JewelryRatings");
        }
    }
}
