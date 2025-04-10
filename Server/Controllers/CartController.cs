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
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<List<CartProductDTO>>> GetCartProduct([FromBody] List<CardItem> lista_items)
        {
            var result = await _cartService.GetCartProductsAsync(lista_items);
            return Ok(result);
        }
    }
}