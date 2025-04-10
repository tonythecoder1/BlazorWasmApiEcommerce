using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Client.Services;
using Server.Services;
using Shared;

namespace Client.Services
{
    public class CartServiceClient : ICartServiceClient
    {
        private readonly HttpClient _httpClient;
        protected ILocalStorageService _localStorageService;

        public CartServiceClient(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            this._localStorageService = localStorageService;
            this._httpClient = httpClient;
        }
        public event Action OnChange;

        public async Task AddCart(CardItem cardItem)
        {
            var carrinho = await _localStorageService.GetItemAsync<List<CardItem>>("cart");  //criamos com a chave cart

            if(carrinho == null){
                carrinho = new List<CardItem>();
            } 
             
            var sameItem = carrinho.Find(p=> p.ProductId == cardItem.ProductId && p.ProductTypeId == cardItem.ProductTypeId);   
            if(sameItem == null){
                carrinho.Add(cardItem);
            } else{
                sameItem.Quantidade += cardItem.Quantidade;
            }


            await _localStorageService.SetItemAsync("cart", carrinho);

            OnChange?.Invoke();
        }

        public async Task<List<CardItem>> GetCartItems(){

            var carrinho = await _localStorageService.GetItemAsync<List<CardItem>>("cart");
            if(carrinho == null){
                carrinho = new List<CardItem>();
            }

            return carrinho;
        }

        public async Task<List<CartProductDTO>> GetCartProductsDto()
        {
            var cartItems = await _localStorageService.GetItemAsync<List<CardItem>>("cart");
            var response = await _httpClient.PostAsJsonAsync("api/cart/products", cartItems);

            if(response == null){
                return new List<CartProductDTO>{ new CartProductDTO() };
            } 

            var cartProductsDto_Response = await response.Content.ReadFromJsonAsync<List<CartProductDTO>>();


            return cartProductsDto_Response??new List<CartProductDTO>();
        }

        public async Task RemoveItemsFromCart(int ProductId, int ProductTypeId)
        {
            var lista_carrinho_local = await _localStorageService.GetItemAsync<List<CardItem>>("cart");

            if(lista_carrinho_local == null){
                return;
            } 

            var cartItem = lista_carrinho_local.Find(x=>x.ProductId == ProductId && x.ProductTypeId == ProductTypeId);

            if(cartItem != null){
                lista_carrinho_local.Remove(cartItem);
                await _localStorageService.SetItemAsync("cart", lista_carrinho_local);
                OnChange.Invoke();
            }
        }

        public async Task UpdateQuantidade(CartProductDTO productDTO)
        {
            var carrinho = await _localStorageService.GetItemAsync<List<CardItem>>("cart");
            if (carrinho == null){
                return;
            }

            var itemEncontrado = carrinho.Find(i=>i.ProductId == productDTO.ProductId 
            && i.ProductTypeId == productDTO.ProductTypeId);

            if(itemEncontrado != null){
                itemEncontrado.Quantidade = productDTO.Quantidade;  //ficou com referencia 
                await _localStorageService.SetItemAsync("cart", carrinho);
            }
        }
    }
}