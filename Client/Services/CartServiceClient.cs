using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Server.Services;
using Shared;

namespace Client.Services
{
    public class CartServiceClient : ICartServiceClient
    {
        private readonly HttpClient _httpClient;
        protected ILocalStorageService _localStorageService;
        protected readonly AuthenticationStateProvider _authenticationStateProvider;

        public CartServiceClient(ILocalStorageService localStorageService, HttpClient httpClient,
            AuthenticationStateProvider authenticationState)
        {
            this._localStorageService = localStorageService;
            this._httpClient = httpClient;
            this._authenticationStateProvider = authenticationState;
        }
        public event Action OnChange;

        public async Task AddCart(CardItem cardItem)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirst("nameid")?.Value;
                cardItem.UserId = userId; // ✅ adiciona aqui
                Console.WriteLine("User is authenticated");
            }
            else
            {
                Console.WriteLine("User is not authenticated");
            }

            var carrinho = await _localStorageService.GetItemAsync<List<CardItem>>("cart") ?? new List<CardItem>();

            var sameItem = carrinho.Find(p => p.ProductId == cardItem.ProductId && p.ProductTypeId == cardItem.ProductTypeId);
            if (sameItem == null)
            {
                carrinho.Add(cardItem);
            }
            else
            {
                sameItem.Quantidade += cardItem.Quantidade;
            }

            await _localStorageService.SetItemAsync("cart", carrinho);
            OnChange?.Invoke();
        }


        public async Task<List<CardItem>> GetCartItems()
        {

            var carrinho = await _localStorageService.GetItemAsync<List<CardItem>>("cart");
            if (carrinho == null)
            {
                carrinho = new List<CardItem>();
            }

            return carrinho;
        }

        public async Task<List<CartProductDTO>> GetCartProductsDto()
        {
            var cartItems = await _localStorageService.GetItemAsync<List<CardItem>>("cart"); //buscar no local o carrinho

            if (cartItems == null || !cartItems.Any())
            {
                return new List<CartProductDTO>();
            }

            var response = await _httpClient.PostAsJsonAsync("api/cart/products", cartItems);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Erro ao buscar produtos do carrinho: {response.StatusCode} - {error}");
                return new List<CartProductDTO>();
            }

            var cartProductsDto_Response = await response.Content.ReadFromJsonAsync<List<CartProductDTO>>();
            return cartProductsDto_Response ?? new List<CartProductDTO>();
        }



        public async Task RemoveItemsFromCart(int ProductId, int ProductTypeId)
        {
            var lista_carrinho_local = await _localStorageService.GetItemAsync<List<CardItem>>("cart");

            if (lista_carrinho_local == null)
            {
                return;
            }

            var cartItem = lista_carrinho_local.Find(x => x.ProductId == ProductId && x.ProductTypeId == ProductTypeId);

            if (cartItem != null)
            {
                lista_carrinho_local.Remove(cartItem);
                await _localStorageService.SetItemAsync("cart", lista_carrinho_local);
                OnChange.Invoke();
            }
        }

        public async Task UpdateQuantidade(CartProductDTO productDTO)
        {
            var carrinho = await _localStorageService.GetItemAsync<List<CardItem>>("cart");
            if (carrinho == null)
            {
                return;
            }

            var itemEncontrado = carrinho.Find(i => i.ProductId == productDTO.ProductId
            && i.ProductTypeId == productDTO.ProductTypeId);

            if (itemEncontrado != null)
            {
                itemEncontrado.Quantidade = productDTO.Quantidade;  //ficou com referencia 
                await _localStorageService.SetItemAsync("cart", carrinho);
            }
        }

        public async Task<List<CardItem>> StoreCartItem(bool emptyLocalCart = false)
        {
            var localCart = await _localStorageService.GetItemAsync<List<CardItem>>("cart");

            if (localCart == null || !localCart.Any())
            {
                return new List<CardItem>();
            }

            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!user.Identity.IsAuthenticated)
            {
                Console.WriteLine("Usuário não está autenticado, não enviando para backend.");
                return localCart;
            }

            // 👉 FALTA ISSO:
            var userId = user.FindFirst("nameid")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                foreach (var item in localCart)
                {
                    item.UserId = userId;
                }
            }

            var response = await _httpClient.PostAsJsonAsync("api/cart/store", localCart);

            if (response.IsSuccessStatusCode)
            {
                var enrichedProducts = await response.Content.ReadFromJsonAsync<List<CartProductDTO>>();

                if (emptyLocalCart)
                {
                    await _localStorageService.RemoveItemAsync("cart");
                }

                await _localStorageService.SetItemAsync("cartProducts", enrichedProducts);

                return localCart;
            }
            else
            {
                var erro = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Erro ao armazenar carrinho: {erro}");
                return new List<CardItem>();
            }
        }


    }
}