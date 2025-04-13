using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Shared;

namespace Client.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;
        public ProductService(HttpClient http)
        {
            _http = http;
        }
        public List<Produto> Lista { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Produto> Lista_produtos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<List<Produto>> GetProdutos()
        {
            try
            {
                var response = await _http.GetAsync("api/Produto/featured");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro HTTP ({response.StatusCode}): {content}");
                    return new List<Produto>();
                }

                return System.Text.Json.JsonSerializer.Deserialize<List<Produto>>(content, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Produto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter produtos: {ex.Message}");
                return new List<Produto>();
            }
        }




        public async Task<List<Produto>> GetProdutosByCategoria(string categoriaName)
        {
            var result = await _http.GetFromJsonAsync<List<Produto>>($"api/produto/por-categoria/{categoriaName}");
            return result ?? new List<Produto>();
        }

        public async Task<Produto> GetProdutosById(int productId)
        {
            var result = await _http.GetFromJsonAsync<Produto>($"api/produto/{productId}");
            return result ?? new Produto();
        }


        public async Task<ProdutoSearchDTO> GetSearchText(string searchText, int page)
        {
            var result = await _http.GetFromJsonAsync<ProdutoSearchDTO>($"api/produto/search/{searchText}/{page}");
            return result ?? new ProdutoSearchDTO();
        }


        public async Task<List<string>> GetSearchTextSuggestions(string searchTextSuggestions)
        {
            var result = await _http.GetFromJsonAsync<List<string>>($"api/Produto/search-suggestions/{searchTextSuggestions}");
            return result ?? new List<string>();
        }
    }
}