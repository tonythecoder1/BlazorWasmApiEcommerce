using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared
{
    public class CardItem
    {
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int Quantidade { get; set; } = 1;
    }
}