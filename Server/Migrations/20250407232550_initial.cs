using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorComAPI.Server.Migrations
{
    public partial class initial : Migration
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
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias_TBL", x => x.Id);
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
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
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

            migrationBuilder.InsertData(
                table: "Categorias_TBL",
                columns: new[] { "Id", "CategoriaId", "Name", "Url" },
                values: new object[] { 1, 0, "Books", "books" });

            migrationBuilder.InsertData(
                table: "Categorias_TBL",
                columns: new[] { "Id", "CategoriaId", "Name", "Url" },
                values: new object[] { 2, 0, "Movies", "movies" });

            migrationBuilder.InsertData(
                table: "Categorias_TBL",
                columns: new[] { "Id", "CategoriaId", "Name", "Url" },
                values: new object[] { 3, 0, "Video Games", "video-games" });

            migrationBuilder.InsertData(
                table: "Produtos_TBL",
                columns: new[] { "Id", "CategoriaId", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 1, 2, "Teclado com switches azuis", "https://via.placeholder.com/150", 59.99m, "Teclado Mecânico" });

            migrationBuilder.InsertData(
                table: "Produtos_TBL",
                columns: new[] { "Id", "CategoriaId", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 2, 2, "Mouse com DPI ajustável e LED RGB", "https://via.placeholder.com/150", 29.99m, "Rato Gamer" });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_TBL_CategoriaId",
                table: "Produtos_TBL",
                column: "CategoriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos_TBL");

            migrationBuilder.DropTable(
                name: "Categorias_TBL");
        }
    }
}
