namespace JewelryShop.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CreateFullTextCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
               sql: "CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;",
               suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FULLTEXT CATALOG ftCatalog");
        }
    }
}
