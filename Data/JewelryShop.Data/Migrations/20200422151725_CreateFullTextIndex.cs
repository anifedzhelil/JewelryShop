namespace JewelryShop.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class CreateFullTextIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON Jewelry(Name, Description) KEY INDEX PK_Jewelry;",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
