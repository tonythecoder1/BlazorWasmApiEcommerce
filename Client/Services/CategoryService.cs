using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared;

namespace Client.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;

        public CategoryService(HttpClient http)
        {
            this.httpClient = http;
        }

        public async Task<List<Categoria>> GetCategorias()
        {
            var result = await httpClient.GetFromJsonAsync<List<Categoria>>("/api/categoria");
            return result??new List<Categoria>();
        }
    }
}