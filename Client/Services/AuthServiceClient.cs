using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared;

namespace Client.Services
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthServiceClient(HttpClient httpClient, ILocalStorageService localStorageService,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> Register(RegisterDTO userDto)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/register", userDto);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> LoginUser(UserLoginDTO userLoginDTO)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", userLoginDTO);

            if (!response.IsSuccessStatusCode)
            {
                return "Email ou senha inválidos";
            }

            var token = await response.Content.ReadAsStringAsync();
            var cleanedToken = token.Trim('"'); // Remove aspas caso venham do JSON

            // Armazenar token no localStorage
            await _localStorageService.SetItemAsStringAsync("authToken", cleanedToken);
            Console.WriteLine("Token salvo no LocalStorage!");

            var teste = await _localStorageService.GetItemAsStringAsync("authToken");
            Console.WriteLine($"Token no localStorage após salvar = {teste}");

            // Definir header Authorization do HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cleanedToken);

            // Atualizar estado de autenticação
            if (_authenticationStateProvider is CustomAuthState customAuth)
            {
                customAuth.NotifyUserAuthentication(cleanedToken);
                await Task.Delay(100); // Pequena pausa para garantir atualização do estado (opcional)
            }

            Console.WriteLine($"TOKEN = {cleanedToken}");

            var check = await _localStorageService.GetItemAsStringAsync("authToken");
            Console.WriteLine($"[DEBUG] Token no LocalStorage FINAL = {check}");


            return "Login com sucesso!";
        }
    }
}
