using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Shared;

namespace Client.Services
{
    public interface IProductService
    {
        public Task<List<Produto>> GetProdutos();
        public Task<Produto> GetProdutosById(int productId);
        public List<Produto> Lista_produtos{get; set; }
        public Task<List<Produto>>GetProdutosByCategoria(string categoriaName);
        public Task<ProdutoSearchDTO> GetSearchText(string searchText, int page);
        public Task<List<string>>GetSearchTextSuggestions(string searchTextSuggestions);

    }
}