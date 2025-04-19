using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        
        [AllowAnonymous]
        [HttpPost("products")]
        public async Task<ActionResult<List<CartProductDTO>>> GetCartProduct([FromBody] List<CardItem> lista_items)
        {
            Console.WriteLine("🎯 Recebido no backend:");
            foreach (var item in lista_items)
            {
                Console.WriteLine($"➡️ ProductId={item.ProductId}, Type={item.ProductTypeId}, Qty={item.Quantidade}, UserId={item.UserId}");
            }

            var result = await _cartService.GetCartProductsAsync(lista_items);
            return Ok(result);
        }


        [HttpPost("store")]
        public async Task<ActionResult<List<CartProductDTO>>> StoreCartItems([FromBody] List<CardItem> lista_items)
        {
            var userIdClaim = User.FindFirst("nameid")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            if (lista_items == null || !lista_items.Any())
            {
                return BadRequest("A lista de itens do carrinho está vazia.");
            }

            try
            {

                var result = await _cartService.StoreCartItems(lista_items, userIdClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log opcional aqui
                return StatusCode(500, $"Erro ao armazenar o carrinho: {ex.Message}");
            }
        }

    }
}