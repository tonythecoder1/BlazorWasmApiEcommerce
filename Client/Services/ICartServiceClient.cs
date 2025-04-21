using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services
{
    public interface ICartServiceClient
    {
        event Action OnChange;
        Task AddCart(CardItem cardItem);
        Task<List<CartProductDTO>> GetCartProductsDto();
        Task RemoveItemsFromCart(int ProductId, int ProductTypeId);
        Task UpdateQuantidade(CartProductDTO productDTO);
        Task<List<CardItem>> StoreCartItem(bool empytLocalCart);
        Task GetCartItemsCount();
        public void NotificarMudancaDoCarrinho();




    }
}