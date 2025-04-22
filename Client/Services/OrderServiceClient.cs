using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Server.Services;
using Shared;

namespace Client.Services
{
    public class OrderServiceClient : IOrderServiceClient
    {
        HttpClient _httpClient;
        AuthenticationStateProvider _authenticationStateProvider;
        NavigationManager _navigationManager;

        public OrderServiceClient(HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager)
        {
            this._httpClient = httpClient;
            this._authenticationStateProvider = authenticationStateProvider;
            this._navigationManager = navigationManager;
        }

        public async Task<OrderDetailsResponseDTO> GetOrderDetails(int orderId)
        {
            var response = await _httpClient.GetFromJsonAsync<OrderDetailsResponseDTO>($"/api/order/{orderId}");
            return response ?? new OrderDetailsResponseDTO();
        }

        public async Task<List<OrderFinalDTO>> GetOrders()
        {
            var result = await _httpClient.GetFromJsonAsync<List<OrderFinalDTO>>("api/order");
            return result ?? new List<OrderFinalDTO>();
        }

        public async Task<ClaimsPrincipal> GetUser()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User;
        }

        public async Task PlaceOrder()
        {
            var user = await GetUser();
            if (user.Identity.IsAuthenticated)
            {
                await _httpClient.PostAsync("api/order", null);

            }
            else
            {
                _navigationManager.NavigateTo("login");
            }

        }
    }
}