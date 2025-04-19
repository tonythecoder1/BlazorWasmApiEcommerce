using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services
{
    public interface ICartService
    {
        Task <List<CartProductDTO>> GetCartProductsAsync (List<CardItem> cardItems);
        Task<List<CartProductDTO>> StoreCartItems(List<CardItem> cardItems, string UserId);
    }
}