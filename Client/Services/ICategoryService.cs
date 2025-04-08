using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Client.Services
{
    public interface ICategoryService
    {
        public Task<List<Categoria>> GetCategorias();
       
    }
}