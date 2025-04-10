using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class CartProductDTO
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantidade { get; set; }
    }
}