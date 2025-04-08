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

        // ✅ Corrige nome do parâmetro (sem underline)
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
    }
}
