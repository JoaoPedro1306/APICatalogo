using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _repository;
        private readonly ILogger _logger;
        public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetProdutos().ToList();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados.");
            }

            return Ok(produtos);
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _repository.GetProduto(id);

            if (produto is null)
            {
                _logger.LogWarning($"Produto com id = {id} não encontrado.");
                return NotFound("Produto não encontrado.");
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
            {
                _logger.LogWarning($"Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            var novoProduto = _repository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning("Dados inválidos.");
                return BadRequest("Dados inválidos.");
            }

            bool atualizado = _repository.Update(produto);

            if (atualizado)
                return Ok(produto);

            return StatusCode(500, $"Falha ao atualizar o produto de id = {id}.");
        }

        [HttpDelete("{id:int:min(1)}")]

        public ActionResult Delete(int id)
        {
            bool deletado = _repository.Delete(id);
            if (deletado)
                return Ok($"Produto de id = {id} foi excluído.");

            return StatusCode(500, $"Falha ao excluir o produto de id={id}.");
        }
    }
}
