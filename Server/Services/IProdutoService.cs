using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Shared;

namespace Server.Services
{
    public interface IProdutoService
    {
        Task<List<Produto>> GetProdutosAsync();
        Task<Produto> GetProdutosAsynById(int ProdutoId);
        Task<List<Produto>> GetProdutotByCategoria(string Url);
        Task<List<Produto>> SearchProductsAsync(string searchText);
        Task<List<string>> GetProductSearchSuggestions(string searchText);
        Task<List<Produto>> GetFeaturedProducts();
    }
}
