using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared;

namespace Server.Services
{
    public class CartService : ICartService
    {
        protected readonly DbContextServer _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(DbContextServer dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
        }


        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirst("nameid")?.Value;
        }

        public async Task<int> GetCartItemsDTOCounter()
        {
            var userId = GetUserId();
            Console.WriteLine("➡️ User ID: " + userId);
            var count = await _dbContext.CardItems_TBL
                                    .Where(u => u.UserId == userId)
                                    .SumAsync(u => u.Quantidade);

            return count;
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

        public async Task<List<CartProductDTO>> StoreCartItems(List<CardItem> cardItems)
        {
            cardItems.ForEach(carti => carti.UserId = GetUserId());
            _dbContext.CardItems_TBL.AddRange(cardItems);
            await _dbContext.SaveChangesAsync();

            return await GetCartProductDTOs();
        }

        public async Task<List<CartProductDTO>> GetCartProductDTOs()
        {
            var userId = GetUserId();
            List<CardItem> lista_items = await _dbContext.CardItems_TBL.Where(c => c.UserId == userId).ToListAsync();
            return await GetCartProductsAsync(lista_items);
        }

        public async Task<bool> AddToCart(CardItem cardItem)
        {
            cardItem.UserId = GetUserId();

            var sameItem = await _dbContext.CardItems_TBL.FirstOrDefaultAsync(c =>
                            c.ProductId == cardItem.ProductId && c.ProductTypeId == cardItem.ProductTypeId && c.UserId == cardItem.UserId);

            if (sameItem == null)
            {
                _dbContext.CardItems_TBL.Add(cardItem);
            }
            else
            {
                sameItem.Quantidade += cardItem.Quantidade; //adicionamos a quantidade ja existente
            }

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCart(CardItem cardItem)
        {
            var exitentItem = await _dbContext.CardItems_TBL.FirstOrDefaultAsync(c => c.ProductId == cardItem.ProductId &&
                c.ProductTypeId == cardItem.ProductTypeId && c.UserId == GetUserId());

            if (exitentItem == null)
            {
                return false;
            }

            exitentItem.Quantidade = cardItem.Quantidade;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveItemFromCart(int productId, int ProductTypeId)
        {
            var result = await _dbContext.CardItems_TBL.FirstOrDefaultAsync(c =>
                c.ProductId == productId && c.ProductTypeId == ProductTypeId && c.UserId == GetUserId());

            if (result != null)
            {
                _dbContext.CardItems_TBL.Remove(result);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            
            return false;

        }
    }
}