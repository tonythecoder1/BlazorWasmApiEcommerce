using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services
{
    public class OrderDetailsResponseDTO
    {
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderDetailsProductDTO> Produtos { get; set; }

    }
}