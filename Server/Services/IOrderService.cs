using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services
{
    public interface IOrderService
    {
        Task<bool> PlaceOrder();
        Task<List<OrderFinalDTO>> GetOrders();
        Task<OrderDetailsResponseDTO> GetOrdersDetails(int Id);
    }
}