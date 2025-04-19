using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared;

namespace Server.Services
{
    public class CartService : ICartService
    {
        protected readonly DbContextServer _dbContext;
        public CartService(DbContextServer dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<List<CartProductDTO>> GetCartProductsAsync(List<CardItem> cardItems) //Converte os itens em CartProductDTO:
        {
            var result = new List<CartProductDTO>();

            foreach (var item in cardItems)
            {

                var product = await _dbContext.Produtos_TBL
                                                .Where(p => p.Id == item.ProductId)
                                                .FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _dbContext.ProdutoVariante_TBL
                                            .Include(v => v.productType)
                                            .Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)
                                            .FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                var cartProduct = new CartProductDTO
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    ProductType = productVariant.productType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Price = productVariant.Price,
                    Quantidade = item.Quantidade
                };

                result.Add(cartProduct);
            }

            return result;
        }

        public async Task<List<CartProductDTO>> StoreCartItems(List<CardItem> cardItems, string UserId)
        {
            // Adiciona o UserId em todos os itens
            cardItems.ForEach(cardItem => cardItem.UserId = UserId);

            // Log para debug
            foreach (var item in cardItems)
            {
                Console.WriteLine($"📦 Enviando item: ProductId={item.ProductId}, Tipo={item.ProductTypeId}, UserId={item.UserId}");
            }

            // Salva os itens no banco
            _dbContext.CardItems_TBL.AddRange(cardItems);
            await _dbContext.SaveChangesAsync();

            // Retorna os dados para exibir no carrinho
            var savedItems = await _dbContext.CardItems_TBL
                .Where(c => c.UserId == UserId)
                .ToListAsync();

            return await GetCartProductsAsync(savedItems);
        }

    }
}