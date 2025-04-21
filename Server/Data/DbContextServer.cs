using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared;

// ...

namespace Server.Data
{
    public class DbContextServer : IdentityDbContext<MyUser>
    {
        public DbContextServer(DbContextOptions<DbContextServer> options) : base(options)
        {
        }

        public DbSet<Order> Orders_TBL { get; set; }
        public DbSet<OrderItem> OrderItems_TBL { get; set; }
        public DbSet<Produto> Produtos_TBL { get; set; } = null!;
        public DbSet<Categoria> Categorias_TBL { get; set; } = null!;
        public DbSet<ProductVariant> ProdutoVariante_TBL { get; set; } = null!;
        public DbSet<ProductType> ProductType_TBL { get; set; } = null!;
        public DbSet<CardItem> CardItems_TBL { get; set; }


        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.Entity<Order>()
               .HasMany(o => o.OrderItems)  // Um Order TEM MUITOS OrderItems
               .WithOne(o => o.Order)       // Cada OrderItem TEM UM Order
               .HasForeignKey(o => o.OrderId);

            model.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ProductId, oi.ProductTypeId });

            model.Entity<CardItem>()
                .HasKey(ci => new { ci.UserId, ci.ProductId, ci.ProductTypeId });

            model.Entity<ProductVariant>()   // Table de jonction avec clé composite
                .HasKey(p => new { p.ProductId, p.ProductTypeId }); // Clé primaire composite

            model.Entity<ProductVariant>()
                 .HasOne(p => p.produto_nav)
                 .WithMany(p => p.productVariants)
                 .HasForeignKey(p => p.ProductId);

            model.Entity<ProductVariant>()
                  .HasOne(p => p.productType)
                  .WithMany(p => p.ProductVariants)
                  .HasForeignKey(p => p.ProductTypeId);





            model.Entity<ProductType>().HasData(
                new ProductType { Id = 1, Name = "Standard" },
                new ProductType { Id = 2, Name = "Petit format" },
                new ProductType { Id = 3, Name = "Format moyen" },
                new ProductType { Id = 4, Name = "Grand format" }
            );

            // Catégories
            model.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Name = "Livres", Url = "books" },
                new Categoria { Id = 2, Name = "Films", Url = "movies" },
                new Categoria { Id = 3, Name = "Jeux vidéo", Url = "video-games" },
                new Categoria { Id = 4, Name = "Périphériques", Url = "peripheriques" }
            );


            // Produits
            model.Entity<Produto>().HasData(
                new Produto
                {
                    Id = 1,
                    Title = "Clavier mécanique",
                    Description = "Clavier avec interrupteurs bleus",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 4,
                    featured = true
                },
                new Produto
                {
                    Id = 2,
                    Title = "Souris gamer",
                    Description = "Souris avec DPI ajustable et LED RGB",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 4,
                    featured = false
                },
                new Produto
                {
                    Id = 3,
                    Title = "Livre : Clean Code",
                    Description = "Un guide de bonnes pratiques de programmation par Robert C. Martin.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 1,
                    featured = false
                },
                new Produto
                {
                    Id = 4,
                    Title = "Livre : Domain-Driven Design",
                    Description = "Traite de la modélisation logicielle orientée domaine.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 1,
                    featured = false
                },
                new Produto
                {
                    Id = 5,
                    Title = "Film : Inception",
                    Description = "Un thriller de science-fiction réalisé par Christopher Nolan.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2,
                    featured = true
                },
                new Produto
                {
                    Id = 6,
                    Title = "Film : Interstellar",
                    Description = "Exploration spatiale pour sauver l’humanité.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 2,
                    featured = true
                },
                new Produto
                {
                    Id = 7,
                    Title = "Jeu : The Witcher 3",
                    Description = "RPG en monde ouvert avec une narration immersive.",
                    ImageUrl = "https://via.placeholder.com/150",
                    CategoriaId = 3,
                    featured = true
                },
                new Produto
                {
                    Id = 8,
                    Title = "God of War Ragnarok",
                    Description = "Kratos dans son aventure épique au cœur de la mythologie nordique.",
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
                    Name = "Clavier Standard",
                    Price = 99.99m,
                    OriginalPrice = 149.99m
                },
                new ProductVariant
                {
                    ProductId = 1,
                    ProductTypeId = 3,
                    Name = "Clavier Moyen",
                    Price = 109.99m,
                    OriginalPrice = 159.99m
                },
                new ProductVariant
                {
                    ProductId = 2,
                    ProductTypeId = 2,
                    Name = "Souris Petite",
                    Price = 59.99m,
                    OriginalPrice = 89.99m
                },
                new ProductVariant
                {
                    ProductId = 2,
                    ProductTypeId = 4,
                    Name = "Souris Grande",
                    Price = 69.99m,
                    OriginalPrice = 99.99m
                }
            );
        }
    }
}
