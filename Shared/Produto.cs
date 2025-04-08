using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace BlazorComAPI.Shared
{
    public class Produto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal? Price { get; set; } 
        public Categoria? categoria{ get; set; } //navegacao N:1
        public int CategoriaId { get; set; }    
    }
}