using Blog.ViewModels;
using MERCEARIA.Data;
using MERCEARIA.Models;
using MERCEARIA.ViewModels;
using MERCEARIA.ViewModels.EstoqueVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MERCEARIA.Controllers
{
    [ApiController]
    [Route("v1/estoques")]
    public class EstoqueController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context)
        {
            try
            {
                var estoque = await context.Estoque.Include(x => x.Produto).ToListAsync();
                if (estoque == null)
                {
                    return NotFound(new ResultViewModel<Produto>("Não nada cadastrado no estoque"));
                }
                
                var vms = new List<VisualizarEstoqueViewModel>();

                foreach(var item in estoque)
                {
                    var vm = new VisualizarEstoqueViewModel()
                    {
                        IdProduto = item.Produto.Id,
                        NomeProduto = item.Produto.NomeProduto,
                        Quantidade = item.Quantidade
                    };
                    vms.Add(vm);
                }
                return Ok(new ResultViewModel<List<VisualizarEstoqueViewModel>>(vms));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Estoque>("03x01 - Falha interna no servidor"));
            }
        }
        [HttpGet("{idProduto:int}")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context, [FromRoute] int idProduto)
        {
            try
            {
                var estoque = await context.Estoque.Include(x => x.Produto).FirstOrDefaultAsync(x => x.Produto.Id == idProduto);
                if (estoque == null)
                {
                    return NotFound(new ResultViewModel<Estoque>("Produto não encontrado"));
                }
                var vm = new VisualizarEstoqueViewModel()
                {
                    IdProduto = estoque.Produto.Id,
                    NomeProduto = estoque.Produto.NomeProduto,
                    Quantidade = estoque.Quantidade
                };
                return Ok(new ResultViewModel<VisualizarEstoqueViewModel>(vm));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarEstoqueViewModel>("03x02- Falha interna no servidor"));
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromServices] MerceariaDataContext context, [FromQuery] CadastrarAlterarEstoqueViewModel vm)
        {
            try
            {
                var produto = context.Produtos.FirstOrDefault(x => x.Id == vm.IdProduto);
                if (produto == null)
                    return NotFound(new ResultViewModel<Produto>("Produto não cadastrado"));

                var estoques = await context.Estoque.FirstOrDefaultAsync(x => x.Produto.Id == vm.IdProduto);
                if (estoques != null)
                    return NotFound(new ResultViewModel<Produto>("Já existe esse produto cadastrado no estoque"));

                var estoque = new Estoque()
                {
                    Id = 0,
                    Produto = produto,
                    Quantidade = vm.Quantidade
                };
                await context.Estoque.AddAsync(estoque);
                await context.SaveChangesAsync();

                return Created($"v1/clientes/{estoque.Id}", new ResultViewModel<Estoque>(estoque));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("03x03 - Falha interna no servidor"));
            }
        }
        [HttpPut("({idProduto:int})")]
        public async Task<IActionResult> PutAsync([FromServices] MerceariaDataContext context, [FromQuery] CadastrarAlterarEstoqueViewModel vm)
        {
            try
            {
                var produto = context.Produtos.FirstOrDefault(x => x.Id == vm.IdProduto);
                if (produto == null)
                    return NotFound(new ResultViewModel<Produto>("Produto não cadastrado"));

                var estoque = await context.Estoque.FirstOrDefaultAsync(x => x.Id == vm.IdProduto);
                if (estoque == null)
                    return NotFound(new ResultViewModel<Produto>("Produto não cadastrado no estoque"));

                estoque.Produto = produto;
                estoque.Quantidade = vm.Quantidade;

                context.Estoque.Update(estoque);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<CadastrarAlterarEstoqueViewModel>(vm));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("03x04 - Não voi possivel alterar o estoque"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("03x05 - Falha interna no servidor"));
            }
        }
        [HttpDelete("{idProduto:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] MerceariaDataContext context, [FromRoute] int idProduto)
        {
            try
            {
                var produto = context.Produtos.FirstOrDefault(x => x.Id == idProduto);
                if (produto == null)
                    return NotFound(new ResultViewModel<Produto>("Produto não cadastrado"));

                var estoque = await context.Estoque.FirstOrDefaultAsync(x => x.Produto.Id == idProduto);
                if (estoque == null)
                    return NotFound(new ResultViewModel<Produto>("Produto não cadastrado no estoque"));


                context.Estoque.Remove(estoque);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Estoque>(estoque));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("03x06 - Não voi possivel excluir o produto do estoque"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("03x07 - Falha interna no servidor"));
            }
        }
    }
}
