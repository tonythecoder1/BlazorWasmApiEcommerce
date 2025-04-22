using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class OrderDetailsProductDTO
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ProductType { get; set; }
        public string ImageUrl { get; set; }
        public int Quantidade { get; set; }
        public decimal TotalPreco { get; set; }
    }
}