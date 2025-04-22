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
    public class OrderController : ControllerBase
    {
        protected readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            this._orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var order = await _orderService.PlaceOrder();
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderFinalDTO>>> GetOrderSumario()
        {
            var orderSummary = await _orderService.GetOrders();
            return Ok(orderSummary);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<List<OrderFinalDTO>>> GetOrderDetils(int orderId)
        {
            var orderDetails = await _orderService.GetOrdersDetails(orderId);
            return Ok(orderDetails);
        }
    }
}