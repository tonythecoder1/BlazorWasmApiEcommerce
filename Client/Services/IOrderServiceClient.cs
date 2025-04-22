using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Services;
using Shared;

namespace Client.Services
{
    public interface IOrderServiceClient
    {
        Task PlaceOrder();
        Task<List<OrderFinalDTO>> GetOrders();
        Task<OrderDetailsResponseDTO> GetOrderDetails(int orderId);
    }
}