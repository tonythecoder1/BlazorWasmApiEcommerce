using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
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

        public AuthServiceClient(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClientFactory.CreateClient("API");
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

            // Atualizar estado de autenticação
            if (_authenticationStateProvider is CustomAuthState customAuth)
            {
                customAuth.NotifyUserAuthentication(cleanedToken);
                await Task.Delay(100); // Pequena pausa para garantir atualização do estado (opcional)
            }

            return "Login com sucesso!";
        }


        public async Task<bool> ChangePassword(UserChangePassword model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/change", model);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = await response.Content.ReadFromJsonAsync<bool>();
                    return result;
                }
                catch (Exception ex)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Erro ao fazer parse do JSON: {ex.Message}");
                    Console.WriteLine($"Conteúdo bruto: {raw}");
                }
            }
            else
            {
                Console.WriteLine($"Erro HTTP: {response.StatusCode}");
                var erro = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Detalhe: {erro}");
            }

            return false;
        }




    }
}
