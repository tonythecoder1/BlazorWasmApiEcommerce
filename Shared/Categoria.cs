using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlazorComAPI.Shared;

namespace Shared
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Produto>? produtos{ get; set; } 
      
    }
}