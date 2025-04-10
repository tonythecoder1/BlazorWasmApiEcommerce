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
        public async Task<List<CartProductDTO>> GetCartProductsAsync(List<CardItem> cardItems)
        {
            var result = new List<CartProductDTO>();

            foreach(var item in cardItems){

                var product = await _dbContext.Produtos_TBL
                                                .Where(p=>p.Id == item.ProductId)
                                                .FirstOrDefaultAsync();
                
                if(product == null){
                    continue;
                }

                var productVariant = await _dbContext.ProdutoVariante_TBL
                                            .Include(v => v.productType)    
                                            .Where(v=> v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)      
                                            .FirstOrDefaultAsync();

                if(productVariant == null){
                    continue;
                }
                
                var cartProduct = new CartProductDTO {
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
    }
}