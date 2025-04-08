using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Server.Data
{
    public class DbContextServer : DbContext
    {
        public DbContextServer(DbContextOptions<DbContextServer> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos_TBL { get; set; } = null!;
        public DbSet<Categoria> Categorias_TBL { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            // Categorias
            model.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Name = "Books", Url = "books" },
                new Categoria { Id = 2, Name = "Movies", Url = "movies" },
                new Categoria { Id = 3, Name = "Video Games", Url = "video-games" }
            );

            // Produtos
            model.Entity<Produto>().HasData(
                new Produto
                {
                    Id = 1,
                    Title = "Teclado Mecânico",
                    Description = "Teclado com switches azuis",
                    Price = 59.99m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2
                },
                new Produto
                {
                    Id = 2,
                    Title = "Rato Gamer",
                    Description = "Mouse com DPI ajustável e LED RGB",
                    Price = 29.99m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2
                },
                new Produto
                {
                    Id = 3,
                    Title = "Livro: Clean Code",
                    Description = "Um guia de boas práticas de programação por Robert C. Martin.",
                    Price = 45.90m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 1
                },
                new Produto
                {
                    Id = 4,
                    Title = "Livro: Domain-Driven Design",
                    Description = "Aborda modelagem de software baseada em domínio.",
                    Price = 68.00m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 1
                },
                new Produto
                {
                    Id = 5,
                    Title = "Filme: Inception",
                    Description = "Um thriller de ficção científica dirigido por Christopher Nolan.",
                    Price = 19.99m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2
                },
                new Produto
                {
                    Id = 6,
                    Title = "Filme: Interstellar",
                    Description = "Exploração espacial para salvar a humanidade.",
                    Price = 24.99m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2
                },
                new Produto
                {
                    Id = 7,
                    Title = "Jogo: The Witcher 3",
                    Description = "RPG de mundo aberto com narrativa envolvente.",
                    Price = 39.90m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 3
                },
                new Produto
                {
                    Id = 8,
                    Title = "Jogo: God of War",
                    Description = "Kratos em sua jornada épica no mundo nórdico.",
                    Price = 49.99m,
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 3
                }
            );
        }
    }
}
