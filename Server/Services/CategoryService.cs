using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared;

namespace Server.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DbContextServer ContextServer;
        public CategoryService(DbContextServer contextServer)
        {
            this.ContextServer = contextServer;
        }
 
        public async Task<List<Categoria>> GetCategorias()
        {
            var categorias = await ContextServer.Categorias_TBL.ToListAsync();
            return categorias;
        }

        public async Task<Categoria> GetCategoriasByName(string CategoriaName)
        {
            //var categorias = await ContextServer.Categorias_TBL.FirstOrDefaultAsync(c => c.Name == CategoriaName);
            var categorias = await ContextServer.Categorias_TBL.Where(c=> c.Name == CategoriaName).FirstAsync();
            return categorias ?? new Categoria();
        }
    }
}