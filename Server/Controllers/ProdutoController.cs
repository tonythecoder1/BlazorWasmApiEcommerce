using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared;

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
        public async Task<ActionResult<List<Produto>>> GetProdutoByCategoria(string categoriaName)
        {
            var produtos = await _produtoService.GetProdutotByCategoria(categoriaName);
            return Ok(produtos);
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Produto>>> GetSearchProducts(string searchText)
        {
            var produtos = await _produtoService.SearchProductsAsync(searchText);
            return Ok(produtos);
        }

        [HttpGet("search-suggestions/{searchSuggestions}")]
        public async Task<ActionResult<List<Produto>>> GetSearchSuggestions(string searchSuggestions)
        {
            var produtos = await _produtoService.GetProductSearchSuggestions(searchSuggestions);
            return Ok(produtos);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<Produto>>> GetFeatured()
        {
            var result = await _produtoService.GetFeaturedProducts();
            return Ok(result);
        }

        [HttpGet("search/{searchText}/{page:int}")]
        public async Task<ActionResult<ProdutoSearchDTO>> GetSearchProductsPaged(string searchText, int page = 1)
        {
            int pageSize = 4;  //define quantos produtos vao aparecer por pagina

            // Pegamos todos os produtos que contêm o texto (podes adaptar com EF direto se quiser mais eficiência)
            var produtosFiltrados = await _produtoService.SearchProductsAsync(searchText);

            int totalProdutos = produtosFiltrados.Count;
            int totalPaginas = (int)Math.Ceiling(totalProdutos / (double)pageSize);

            var produtosPaginados = produtosFiltrados
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dto = new ProdutoSearchDTO
            {
                Lista_produtos_search = produtosPaginados,
                Pages = totalPaginas,
                CurrentPage = page
            };

            return Ok(dto);
        }




    }
}
