using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorComAPI.Server.Migrations
{
    public partial class FeaturedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "featured",
                table: "Produtos_TBL",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 1,
                column: "featured",
                value: true);

            migrationBuilder.UpdateData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 5,
                column: "featured",
                value: true);

            migrationBuilder.UpdateData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 6,
                column: "featured",
                value: true);

            migrationBuilder.UpdateData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 7,
                column: "featured",
                value: true);

            migrationBuilder.UpdateData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Title", "featured" },
                values: new object[] { "God of War", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "featured",
                table: "Produtos_TBL");

            migrationBuilder.UpdateData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 8,
                column: "Title",
                value: "Jogo: God of War");
        }
    }
}
