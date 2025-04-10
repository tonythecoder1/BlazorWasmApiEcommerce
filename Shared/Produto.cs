using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;


public class Produto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public int CategoriaId { get; set; }
    public Categoria? categoria { get; set; }
    public bool featured {get;set;}
    public List<ProductVariant> productVariants { get; set; } = new();
}
