using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Data;
using Shared;

namespace Server.Services
{
    public class OrderService : IOrderService
    {
        protected readonly DbContextServer _dbcontext;
        protected readonly ICartService _cartService;
        protected readonly IHttpContextAccessor _httpContextAcessor;
        public OrderService(DbContextServer dbContextServer, ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            this._cartService = cartService;
            this._httpContextAcessor = httpContextAccessor;
            this._dbcontext = dbContextServer;
        }

        public string GetUserId()
        {
            return _httpContextAcessor.HttpContext.User.FindFirst("nameid")?.Value;
        }
        public async Task<bool> PlaceOrder()
        {
            var produtos = await _cartService.GetCartProductDTOs();
            decimal TotalPreco = 0;
            produtos.ForEach(p => TotalPreco += p.Price * p.Quantidade);

            var litsaOrderItems = new List<OrderItem>();

            produtos.ForEach(p => litsaOrderItems.Add(new OrderItem
            {
                ProductId = p.ProductId,
                ProductTypeId = p.ProductTypeId,
                Quantidade = p.Quantidade,
                TotalPreco = p.Price * p.Quantidade
            }));

            var order = new Order
            {
                UserId = GetUserId(),
                Date = DateTime.Now,
                TotalPrice = TotalPreco,
                OrderItems = litsaOrderItems
            };

            _dbcontext.Orders_TBL.Add(order);
            _dbcontext.CardItems_TBL.RemoveRange(_dbcontext.CardItems_TBL
                                    .Where(x=> x.UserId == GetUserId()));
                                    
            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}