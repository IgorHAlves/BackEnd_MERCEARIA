using Blog.ViewModels;
using MERCEARIA.Data;
using MERCEARIA.Models;
using MERCEARIA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MERCEARIA.Controllers
{
    [ApiController]
    [Route("v1/produtos")]
    public class ProdutoController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context)
        {
            try
            {
                var produtos = await context.Produtos.ToListAsync();
                if (produtos == null)
                {
                    return NotFound(new ResultViewModel<Produto>("Não existem produtos cadastrados"));
                }

                return Ok(new ResultViewModel<List<Produto>>(produtos));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x01 - Falha interna no servidor"));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context, [FromRoute] int id)
        {
            try
            {
                var produto = await context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto == null)
                {
                    return NotFound(new ResultViewModel<Produto>("Produto não encontrado"));
                }
                return Ok(new ResultViewModel<Produto>(produto));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Produto>("02x02- Falha interna no servidor"));
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromServices] MerceariaDataContext context, [FromQuery] ProdutoViewModel vm)
        {
            try
            {
                var produto = new Produto()
                {
                    Id = 0,
                    NomeProduto = vm.NomeProduto,
                    PrecoUnit = vm.PrecoUnit
                };

                await context.Produtos.AddAsync(produto);

                var estoque = new Estoque()
                {
                    Id = 0,
                    Produto = produto,
                    Quantidade = 0
                };
                await context.Estoque.AddAsync(estoque);
                await context.SaveChangesAsync();

                return Created($"v1/clientes/{produto.Id}", new ResultViewModel<Produto>(produto));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x03 - Falha interna no servidor"));
            }
        }
        [HttpPut("({id:int})")]
        public async Task<IActionResult> PutAsync([FromServices] MerceariaDataContext context, [FromRoute] int id, [FromQuery] ProdutoViewModel vm)
        {
            try
            {
                var produto = await context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto == null)
                    return NotFound(new ResultViewModel<Produto>("Produto não encontrado"));

                produto.NomeProduto = vm.NomeProduto;
                produto.PrecoUnit = vm.PrecoUnit;

                context.Produtos.Update(produto);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<ProdutoViewModel>(vm));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x04 - Não foi possivel alterar o produto"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x05 - Falha interna no servidor"));
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] MerceariaDataContext context, [FromRoute] int id)
        {
            try
            {
                var produto = await context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
                if (produto == null)
                    return NotFound(new ResultViewModel<Produto>("Produto não localizado"));
                context.Produtos.Remove(produto);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Produto>(produto));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x06 - Não voi possivel excluir o produto"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x07 - Falha interna no servidor"));
            }
        }
    }
}
