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

        public async Task<List<Produto>> GetFeaturedProducts()
        {
            var produtos = await _dbContextServer.Produtos_TBL.Where(p=>p.featured)
                                                                .Include(p=>p.productVariants)
                                                                .ToListAsync();
            
            return produtos;
                                                               
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var products = await SearchProductsAsync(searchText);
            List<string> suggestions = new List<string>();

            foreach (var product in products){
                if(product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)){
                    suggestions.Add(product.Title);
                }

                if(product.Description != null){
                    var ponctuation = product.Description.Where(char.IsPunctuation)   //Distinct(): Remove duplicatas — só mantém um de cada caractere de pontuação encontrado.
                        .Distinct().ToArray();

                        // Divide a descrição do produto em palavras e remove pontuações do início e fim de cada palavra.
                        //split usa os espcoes como separador, mas depois usamos split para usar a colecao de pontuacoes recolinhas como separador
                    var words = product.Description.Split()
                                       .Select(s => s.Trim(ponctuation));


                    foreach(var w in words){
                        if(w.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !suggestions.Contains(w)){
                            suggestions.Add(w);
                        }
                    }
                       
                }

            }

            return suggestions;
        }

        public async Task<Produto> GetProdutosAsynById(int produtoId)
        {



            var produto = await _dbContextServer.Produtos_TBL        //devolve produtos com o tipo e variantes
                                                 .Include(p => p.productVariants)
                                                 .ThenInclude(p => p.productType)
                                                 .FirstOrDefaultAsync(p => p.Id == produtoId);

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
            return await _dbContextServer.Produtos_TBL
                                        .Include(p => p.productVariants)
                                        .ThenInclude(p => p.productType)
                                        .ToListAsync();
        }

        public async Task<List<Produto>> GetProdutotByCategoria(string Url)
        {
            var produtos = await _dbContextServer.Produtos_TBL
                                    .Include(p => p.categoria)
                                    .Include(p => p.productVariants)
                                    .ThenInclude(v => v.productType)
                                    .Where(p => p.categoria.Url.ToLower() == Url.ToLower())
                                    .ToListAsync();

            return produtos;
        }

        public Task<List<Produto>> SearchProductsAsync(string searchText)
        {
            var response = _dbContextServer.Produtos_TBL
                                            .Where(p=>p.Title.ToLower().Contains(searchText.ToLower())
                                            || p.Description.ToLower().Contains(searchText.ToLower()))
                                            .ToListAsync();

            return response;  
        }
    }
}