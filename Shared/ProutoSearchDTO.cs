using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProdutoSearchDTO
    {
        public List<Produto> Lista_produtos_search {get; set; } = new List<Produto>();
        public int Pages {get; set; } 
        public int CurrentPage { get; set; }
        
    }
}