using System;
using LojaApi.Entities;
using LojaApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LojaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Produto>> GetAll()
        {
            var clientes = ProdutoRepository.GetAll();
            // 200 OK - Sucesso 
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = ProdutoRepository.GetById(id);

            if (produto == null)
            {
                // 404 Not Found - Recurso não encontrado 
                return NotFound();
            }

            // 200 OK - Sucesso 
            return Ok(produto);
        } 
        
        [HttpPost]  
        public ActionResult<Produto> Add(Produto novoProduto)  
        { 
            // Validação simples (o ideal é fazer validações mais complexas) 
            if (string.IsNullOrWhiteSpace(novoProduto.Nome)) 
            { 
                // 400 Bad Request - Erro no cliente (dados inválidos) 
                return BadRequest("O nome do cliente é obrigatório.");  
            } 

            var produtoCriado = ProdutoRepository.Add(novoProduto); 

            // 201 Created - Novo recurso criado com sucesso 
            // Retorna o recurso criado e a URL para acessá-lo (boa prática REST) 
            return CreatedAtAction(nameof(GetById), new { id = produtoCriado.Id }, produtoCriado);  
        } 

    }
}