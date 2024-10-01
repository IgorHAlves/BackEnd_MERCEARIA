using Blog.ViewModels;
using MERCEARIA.Data;
using MERCEARIA.Models;
using MERCEARIA.ViewModels;
using MERCEARIA.ViewModels.PedidoVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace MERCEARIA.Controllers
{
    [ApiController]
    [Route("v1/pedidos")]
    public class PedidosController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context)
        {
            try
            {
                var pedidos = await context.Pedidos.Include(x => x.Cliente).Include(x => x.Itens).ThenInclude(x => x.Produto).ToListAsync();
                if (pedidos == null)
                {
                    return NotFound(new ResultViewModel<Pedido>("Não existem pedidos cadastrados"));
                }

                var vm = new List<VisualizarPedidoViewModel>();

                foreach(var pedido in pedidos)
                {
                    vm.Add(new VisualizarPedidoViewModel()
                    {
                        Cliente = pedido.Cliente,
                        Itens = pedido.Itens.Select(x => new VisualizarPedidoItemViewModel()
                        {
                            IdProduto = x.Produto.Id,
                            NomeProduto = x.Produto.NomeProduto,
                            Quantidade = x.Quantidade
                        }).ToList(),
                        Pago = pedido.Pago
                    });
                }

                return Ok(new ResultViewModel<List<VisualizarPedidoViewModel>>(vm));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarPedidoViewModel>("04x01 - Falha interna no servidor"));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context, [FromRoute] int id)
        {
            try
            {
                var pedido = await context.Pedidos.Include(x => x.Cliente).Include(x=> x.Itens).ThenInclude(x => x.Produto).FirstOrDefaultAsync(x => x.Id == id);
                if (pedido == null)
                {
                    return NotFound(new ResultViewModel<Produto>("Pedido não encontrado"));
                }

                var vm = new VisualizarPedidoViewModel()
                {
                    Cliente = pedido.Cliente,
                    Itens = pedido.Itens.Select(x => new VisualizarPedidoItemViewModel()
                    {
                        IdProduto = x.Produto.Id,
                        NomeProduto = x.Produto.NomeProduto,
                        Quantidade = x.Quantidade
                    }).ToList()
                };

                return Ok(new ResultViewModel<VisualizarPedidoViewModel>(vm));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<VisualizarPedidoViewModel>("04x02- Falha interna no servidor"));
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromServices] MerceariaDataContext context, [FromBody] CadastrarPedidoViewModel vm)
        {
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == vm.IdCliente);
                if (cliente == null)
                    return NotFound(new ResultViewModel<Cliente>("Cliente não cadastrado"));
                if (cliente.Ativo == false) 
                    return BadRequest(new ResultViewModel<Cliente>("O cliente está inativo"));

                var pedido = new Pedido()
                {
                    Cliente = cliente,
                    Pago = vm.Pago
                };
                //conferir estoque
                foreach (var itemVM in vm.Itens)
                {
                    var conferirEstoque = await context.Estoque.Select(x => new { x.Produto, x.Quantidade }).FirstOrDefaultAsync(x => x.Produto.Id == itemVM.IdProduto);
                    if (conferirEstoque == null)
                        return NotFound(new ResultViewModel<Estoque>("Não foi possível localizar esse produto no estoque"));
                    if (itemVM.Quantidade > conferirEstoque.Quantidade)
                        return BadRequest(new ResultViewModel<Estoque>($"Quantidade do Produto com id: {itemVM.IdProduto} indisponível no momento, a quantidade máxima é: {conferirEstoque.Quantidade}"));

                    var produto = await context.Produtos.FirstOrDefaultAsync(x => x.Id == itemVM.IdProduto);

                    var item = new PedidoItem()
                    {
                        Pedido = pedido,
                        Produto = produto,
                        Quantidade = itemVM.Quantidade
                    };

                    pedido.Itens.Add(item);
                    //await context.PedidosItens.AddAsync(item);
                }

                await context.Pedidos.AddAsync(pedido);

                //remover item do estoque
                foreach (var itemVM in vm.Itens)
                {
                    var removerEstoque = await context.Estoque.FirstOrDefaultAsync(x => x.Produto.Id == itemVM.IdProduto);
                    removerEstoque.Quantidade -= itemVM.Quantidade;
                    context.Estoque.Update(removerEstoque);
                }

                await context.SaveChangesAsync();

                var vmRetorno = new VisualizarPedidoViewModel()
                {
                    Cliente = pedido.Cliente,
                    Itens = pedido.Itens.Select(x => new VisualizarPedidoItemViewModel()
                    {
                        IdProduto = x.Produto.Id,
                        NomeProduto = x.Produto.NomeProduto,
                        Quantidade = x.Quantidade,
                    }).ToList(),
                };
                
                return Created($"v1/pedidos/{pedido.Id}", new ResultViewModel<VisualizarPedidoViewModel>(vmRetorno));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("04x03 - Falha interna no servidor"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] MerceariaDataContext context, [FromRoute] int id)
        {
            try
            {
                var pedido = await context.Pedidos.FirstOrDefaultAsync(x => x.Id == id);
                if (pedido == null)
                    return NotFound(new ResultViewModel<Pedido>("Pedido não localizado"));
                context.Pedidos.Remove(pedido);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Pedido>(pedido));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("04x05 - Não foi possivel excluir o pedido"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("04x06 - Falha interna no servidor"));
            }
        }
    }
}
