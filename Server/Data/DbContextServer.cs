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
        public DbSet<ProductVariant> ProdutoVariante_TBL { get; set; } = null!;
        public DbSet<ProductType> ProductType_TBL { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.Entity<ProductVariant>()   //Tabela de Juncao com chave Composta
                .HasKey(p => new { p.ProductId, p.ProductTypeId }); //chave primária composta tabela de juncao

            model.Entity<ProductVariant>()
                 .HasOne(p => p.produto_nav)
                 .WithMany(p => p.productVariants)
                 .HasForeignKey(p => p.ProductId);

            model.Entity<ProductVariant>()
                  .HasOne(p => p.productType)
                  .WithMany(p => p.ProductVariants)
                  .HasForeignKey(p => p.ProductTypeId);

            model.Entity<ProductType>().HasData(
                new ProductType { Id = 1, Name = "Padrão" },
                new ProductType { Id = 2, Name = "Tamanho Pequeno" },
                new ProductType { Id = 3, Name = "Tamanho Médio" },
                new ProductType { Id = 4, Name = "Tamanho Grande" }
            );

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
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2,
                    featured = true
                },
                new Produto
                {
                    Id = 2,
                    Title = "Rato Gamer",
                    Description = "Mouse com DPI ajustável e LED RGB",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2,
                    featured = false
                },
                new Produto
                {
                    Id = 3,
                    Title = "Livro: Clean Code",
                    Description = "Um guia de boas práticas de programação por Robert C. Martin.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 1,
                    featured = false
                },
                new Produto
                {
                    Id = 4,
                    Title = "Livro: Domain-Driven Design",
                    Description = "Aborda modelagem de software baseada em domínio.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 1,
                    featured = false
                },
                new Produto
                {
                    Id = 5,
                    Title = "Filme: Inception",
                    Description = "Um thriller de ficção científica dirigido por Christopher Nolan.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2,
                    featured = true
                },
                new Produto
                {
                    Id = 6,
                    Title = "Filme: Interstellar",
                    Description = "Exploração espacial para salvar a humanidade.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2,
                    featured = true
                },
                new Produto
                {
                    Id = 7,
                    Title = "Jogo: The Witcher 3",
                    Description = "RPG de mundo aberto com narrativa envolvente.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 3,
                    featured = true
                },
                new Produto
                {
                    Id = 8,
                    Title = "God of War",
                    Description = "Kratos em sua jornada épica no mundo nórdico.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 3,
                    featured = true
                }
            );

            model.Entity<ProductVariant>().HasData(
                new ProductVariant
                {
                    ProductId = 1,
                    ProductTypeId = 1,
                    Name = "Teclado Padrão",
                    Price = 99.99m,
                    OriginalPrice = 149.99m
                },
                new ProductVariant
                {
                    ProductId = 1,
                    ProductTypeId = 3,
                    Name = "Teclado Médio",
                    Price = 109.99m,
                    OriginalPrice = 159.99m
                },
                new ProductVariant
                {
                    ProductId = 2,
                    ProductTypeId = 2,
                    Name = "Rato Pequeno",
                    Price = 59.99m,
                    OriginalPrice = 89.99m
                },
            new ProductVariant
            {
                ProductId = 2,
                ProductTypeId = 4,
                Name = "Rato Grande",
                Price = 69.99m,
                OriginalPrice = 99.99m
            }
        );
        }
    }
}
