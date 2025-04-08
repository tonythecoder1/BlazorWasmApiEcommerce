using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorComAPI.Shared;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared;

namespace Server.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly DbContextServer _dbContextServer;
        public ProdutoService(DbContextServer dbContextServer)
        {
            this._dbContextServer = dbContextServer;
        }

        public async Task<Produto> GetProdutosAsynById(int produtoId)
        {
            var produto = await _dbContextServer.Produtos_TBL.FindAsync(produtoId);
            if (produto == null)
            throw new Exception("Produto não encontrado.");

            return produto;
        }

        public async Task<List<Produto>> GetProdutosAsync()
        {
            //var response = new ServiceResponse<List<Produto>>(){
                //Data = await _dbContextServer.Produtos_TBL.ToListAsync()
            //};
            //return response;
            return await _dbContextServer.Produtos_TBL.ToListAsync();
        }

        public async Task<List<Produto>> GetProdutotByCategoria(string Url)
        {
            var produtos = await _dbContextServer.Produtos_TBL
                                                .Include(p=>p.categoria) //chamamos a tabela categoria JOIN
                                                .Where(p=> p.categoria.Url.ToLower() == Url.ToLower())
                                                .ToListAsync();
            return produtos;
        }
    }
}