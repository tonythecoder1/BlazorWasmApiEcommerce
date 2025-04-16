using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Client
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthHeaderHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("[AuthHeaderHandler] Executando handler para a requisição: " + request.RequestUri);

            var token = await _localStorage.GetItemAsStringAsync("authToken");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"[AuthHeaderHandler] Token adicionado ao header: {token.Substring(0, 30)}...");
            }
            else
            {
                Console.WriteLine("[AuthHeaderHandler] Nenhum token encontrado no LocalStorage.");
            }

            var response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine($"[AuthHeaderHandler] Resposta recebida: {(int)response.StatusCode} {response.ReasonPhrase}");

            return response;
        }
    }
}
