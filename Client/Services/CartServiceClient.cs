using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
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

                await _httpClient.PostAsJsonAsync("api/cart/add", cardItem);
            }
            else
            {

                var carrinho = await _localStorageService.GetItemAsync<List<CardItem>>("cart") ?? new List<CardItem>();

                var sameItem = carrinho.Find(p => p.ProductId == cardItem.ProductId && p.ProductTypeId == cardItem.ProductTypeId);
                if (sameItem == null)
                {
                    carrinho.Add(cardItem);
                }
                else
                {
                    sameItem.Quantidade += cardItem.Quantidade; //adicionamos a quantidado a item existente
                }

                await _localStorageService.SetItemAsync("cart", carrinho);
            }

            await GetCartItemsCount();
        }

        public async Task<List<CartProductDTO>> GetCartProductsDto()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var responseGetProductsDto = await _httpClient.GetFromJsonAsync<List<CartProductDTO>>("api/cart");
                return responseGetProductsDto ?? new List<CartProductDTO>();

            }
            else
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

                //converte o json para uma lista de objetos
                var cartProductsDto_Response = await response.Content.ReadFromJsonAsync<List<CartProductDTO>>();
                return cartProductsDto_Response ?? new List<CartProductDTO>();
            }

        }

        public async Task RemoveItemsFromCart(int ProductId, int ProductTypeId)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                await _httpClient.DeleteAsync($"api/cart/{ProductId}/{ProductTypeId}");
            }
            else
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

                }
            }

            await GetCartItemsCount();

        }

        public async Task UpdateQuantidade(CartProductDTO productDTO)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {

                var Request = new CardItem
                {
                    ProductId = productDTO.ProductId,
                    ProductTypeId = productDTO.ProductTypeId,
                    Quantidade = productDTO.Quantidade
                };

                await _httpClient.PutAsJsonAsync("api/cart/update-quantity", Request);
            }
            else
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

            var userId = user.FindFirst("nameid")?.Value;
            Console.WriteLine($" ID do usuário autenticado: {userId}");

            if (string.IsNullOrWhiteSpace(userId))
            {
                Console.WriteLine(" Não foi possível obter o ID do usuário.");
                return localCart;
            }

            foreach (var item in localCart)
            {
                item.UserId = userId;
                Console.WriteLine($" Item preparado para envio: {item.ProductId}, UserId: {item.UserId}");
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

        public async Task GetCartItemsCount()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var result = await _httpClient.GetAsync("api/cart/count");

                if (result.IsSuccessStatusCode)
                {
                    var count = int.Parse(await result.Content.ReadAsStringAsync());
                    await _localStorageService.SetItemAsync("cartItemsCount", count);
                }
            }
            else
            {
                var cart = await _localStorageService.GetItemAsync<List<CardItem>>("cart");
                var count = cart.Sum(x => x.Quantidade);
                await _localStorageService.SetItemAsync("cartItemsCount", count);
            }

            OnChange.Invoke();
        }

        public void NotificarMudancaDoCarrinho()
        {
            OnChange?.Invoke();
        }




    }
}