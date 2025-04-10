using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Shared;

public class ProductVariant
{
    public int ProductId { get; set; }
    public int ProductTypeId { get; set; }
    public string Name { get; set; } = string.Empty;

    public ProductType? productType { get; set; }

    [JsonIgnore]
    public Produto? produto_nav { get; set; }

    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
}

