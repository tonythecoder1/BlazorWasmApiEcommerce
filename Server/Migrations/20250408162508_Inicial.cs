using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorComAPI.Server.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categorias_TBL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias_TBL", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductType_TBL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType_TBL", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Produtos_TBL",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos_TBL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_TBL_Categorias_TBL_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias_TBL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProdutoVariante_TBL",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoVariante_TBL", x => new { x.ProductId, x.ProductTypeId });
                    table.ForeignKey(
                        name: "FK_ProdutoVariante_TBL_ProductType_TBL_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductType_TBL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoVariante_TBL_Produtos_TBL_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Produtos_TBL",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categorias_TBL",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Books", "books" },
                    { 2, "Movies", "movies" },
                    { 3, "Video Games", "video-games" }
                });

            migrationBuilder.InsertData(
                table: "ProductType_TBL",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Padrão" },
                    { 2, "Tamanho Pequeno" },
                    { 3, "Tamanho Médio" },
                    { 4, "Tamanho Grande" }
                });

            migrationBuilder.InsertData(
                table: "Produtos_TBL",
                columns: new[] { "Id", "CategoriaId", "Description", "ImageUrl", "Title" },
                values: new object[,]
                {
                    { 1, 2, "Teclado com switches azuis", "https://via.placeholder.com/150", "Teclado Mecânico" },
                    { 2, 2, "Mouse com DPI ajustável e LED RGB", "https://via.placeholder.com/150", "Rato Gamer" },
                    { 3, 1, "Um guia de boas práticas de programação por Robert C. Martin.", "https://via.placeholder.com/150", "Livro: Clean Code" },
                    { 4, 1, "Aborda modelagem de software baseada em domínio.", "https://via.placeholder.com/150", "Livro: Domain-Driven Design" },
                    { 5, 2, "Um thriller de ficção científica dirigido por Christopher Nolan.", "https://via.placeholder.com/150", "Filme: Inception" },
                    { 6, 2, "Exploração espacial para salvar a humanidade.", "https://via.placeholder.com/150", "Filme: Interstellar" },
                    { 7, 3, "RPG de mundo aberto com narrativa envolvente.", "https://via.placeholder.com/150", "Jogo: The Witcher 3" },
                    { 8, 3, "Kratos em sua jornada épica no mundo nórdico.", "https://via.placeholder.com/150", "Jogo: God of War" }
                });

            migrationBuilder.InsertData(
                table: "ProdutoVariante_TBL",
                columns: new[] { "ProductId", "ProductTypeId", "Name", "OriginalPrice", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Teclado Padrão", 149.99m, 99.99m },
                    { 1, 3, "Teclado Médio", 159.99m, 109.99m },
                    { 2, 2, "Rato Pequeno", 89.99m, 59.99m },
                    { 2, 4, "Rato Grande", 99.99m, 69.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_TBL_CategoriaId",
                table: "Produtos_TBL",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoVariante_TBL_ProductTypeId",
                table: "ProdutoVariante_TBL",
                column: "ProductTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdutoVariante_TBL");

            migrationBuilder.DropTable(
                name: "ProductType_TBL");

            migrationBuilder.DropTable(
                name: "Produtos_TBL");

            migrationBuilder.DropTable(
                name: "Categorias_TBL");
        }
    }
}
