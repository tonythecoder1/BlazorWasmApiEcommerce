using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoryService _CategoriaServices;
        public CategoriaController(ICategoryService categoryService)
        {
            this._CategoriaServices = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetCategorias(){
            var categorias = await _CategoriaServices.GetCategorias();
            return Ok(categorias);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Categoria>> GetCategoriaByname(string name){
            var categoria = await _CategoriaServices.GetCategoriasByName(name);
            return Ok(categoria);
        }
    }
}