using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorComAPI.Server.Migrations
{
    public partial class SeedMoreProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Produtos_TBL",
                columns: new[] { "Id", "CategoriaId", "Description", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 3, 1, "Um guia de boas práticas de programação por Robert C. Martin.", "https://via.placeholder.com/150", 45.90m, "Livro: Clean Code" },
                    { 4, 1, "Aborda modelagem de software baseada em domínio.", "https://via.placeholder.com/150", 68.00m, "Livro: Domain-Driven Design" },
                    { 5, 2, "Um thriller de ficção científica dirigido por Christopher Nolan.", "https://via.placeholder.com/150", 19.99m, "Filme: Inception" },
                    { 6, 2, "Exploração espacial para salvar a humanidade.", "https://via.placeholder.com/150", 24.99m, "Filme: Interstellar" },
                    { 7, 3, "RPG de mundo aberto com narrativa envolvente.", "https://via.placeholder.com/150", 39.90m, "Jogo: The Witcher 3" },
                    { 8, 3, "Kratos em sua jornada épica no mundo nórdico.", "https://via.placeholder.com/150", 49.99m, "Jogo: God of War" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Produtos_TBL",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
