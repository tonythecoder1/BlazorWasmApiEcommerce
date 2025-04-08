using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services
{
    public interface ICategoryService
    {
        Task<List<Categoria>> GetCategorias();
        Task<Categoria> GetCategoriasByName(string CategoriaName);
    }
}