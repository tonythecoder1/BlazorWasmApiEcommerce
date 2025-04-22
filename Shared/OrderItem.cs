using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class OrderItem
    {
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Produto Produto { get; set; }

        [ForeignKey("Produto")]
        public int ProductId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPreco { get; set; }
    }
}