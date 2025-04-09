using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Produto>>> Get()
        {
            var result = await _produtoService.GetProdutosAsync();
            return Ok(result);
        }

        [HttpGet("{produtoId:int}")]
        public async Task<ActionResult<Produto>> GetProdutoById(int produtoId)
        {
            var result = await _produtoService.GetProdutosAsynById(produtoId);
            return Ok(result);
        }

        [HttpGet("por-categoria/{categoriaName}")]
        public async Task<ActionResult<List<Produto>>> GetProdutoByCategoria(string categoriaName){
            var produtos = await _produtoService.GetProdutotByCategoria(categoriaName);
            return Ok(produtos);
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Produto>>> GetSearchProducts(string searchText){
            var produtos = await _produtoService.SearchProductsAsync(searchText);
            return Ok(produtos);
        }

        [HttpGet("search-suggestions/{searchSuggestions}")]
        public async Task<ActionResult<List<Produto>>> GetSearchSuggestions(string searchSuggestions){
            var produtos = await _produtoService.GetProductSearchSuggestions(searchSuggestions);
            return Ok(produtos);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<Produto>>> GetFeatured()
        {
            var result = await _produtoService.GetFeaturedProducts();
            return Ok(result);
        }
    }
}
