using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //sumario de orders
        public async Task<List<OrderFinalDTO>> GetOrders()
        {
            var listaOrderFinal = new List<OrderFinalDTO>();
            var result = await _dbcontext.Orders_TBL
                            .Include(oi => oi.OrderItems) //
                            .ThenInclude(p => p.Produto)
                            .Where(o => o.UserId == GetUserId())
                            .OrderByDescending(o => o.Date)
                            .ToListAsync();

            foreach (var order in result)
            {
                //sumario
                OrderFinalDTO OrderDto = new OrderFinalDTO
                {
                    Id = order.Id,
                    Date = DateTime.Now,
                    TotalPrice = order.TotalPrice,
                    Produto = order.OrderItems.Any()
                    ? (order.OrderItems.Count > 1
                    ? $"{order.OrderItems.First().Produto.Title} and {order.OrderItems.Count - 1} more..."
                    : order.OrderItems.First().Produto.Title)
                    : "Sem produtos",
                    ProdutoImgUrl = order.OrderItems.FirstOrDefault()?.Produto?.ImageUrl ?? "img/default.png"

                };

                listaOrderFinal.Add(OrderDto);
            }

            return listaOrderFinal;

        }

        public async Task<OrderDetailsResponseDTO> GetOrdersDetails(int Id)
        {
            var order = await _dbcontext.Orders_TBL
                                    .Include(o => o.OrderItems)
                                    .ThenInclude(o => o.Produto)  //para cada OrderItem, inclui também o Produto associado
                                    .Include(o => o.OrderItems)
                                    .ThenInclude(o => o.ProductType) //para cada OrderItem, inclui também o ProductType associado
                                    .Where(o => o.UserId == GetUserId() && o.Id == Id)
                                    .OrderByDescending(o => o.Date)
                                    .FirstOrDefaultAsync();

            var orderDetailsResponse = new OrderDetailsResponseDTO
            {
                Date = DateTime.Now,
                TotalPrice = order.TotalPrice,
                Produtos = new List<OrderDetailsProductDTO>()
            };

            foreach (var item in order.OrderItems)
            {
                orderDetailsResponse.Produtos.Add(new OrderDetailsProductDTO
                {
                    ProductId = item.ProductId,
                    ImageUrl = item.Produto.ImageUrl,
                    ProductType = item.ProductType.Name,
                    Quantidade = item.Quantidade,
                    Title = item.Produto.Title,
                    TotalPreco = item.TotalPreco
                });
            }

            return orderDetailsResponse;

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
                                    .Where(x => x.UserId == GetUserId()));

            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}