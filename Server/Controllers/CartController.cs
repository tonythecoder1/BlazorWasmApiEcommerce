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

            try
            {
                var result = await _cartService.StoreCartItems(lista_items);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log opcional aqui
                return StatusCode(500, $"Erro ao armazenar o carrinho: {ex.Message}");
            }
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCartCount()
        {
            return await _cartService.GetCartItemsDTOCounter();
        }

        [HttpGet]
        public async Task<ActionResult<List<CartProductDTO>>> GetDbCartProducts()
        {
            var result = await _cartService.GetCartProductDTOs();
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<bool>> AddToCartDb(CardItem listaCartItem)
        {
            var result = await _cartService.AddToCart(listaCartItem);

            if (!result)
            {
                return BadRequest("Nao foi possivel adicionar no carrinho");
            }

            return Ok(result);
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<bool>> UpdateCartQuantity(CardItem listaCartItem)
        {
            var result = await _cartService.UpdateCart(listaCartItem);

            if (!result)
            {
                return BadRequest("Nao foi possivel atulizar a quantidade no carrinho");
            }

            return Ok(result);
        }

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult> DeleteCartItem(int productId, int productTypeId)
        {
            var result = await _cartService.RemoveItemFromCart(productId, productTypeId);
            return Ok(result);
        }

    }
}