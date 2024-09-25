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

                return Ok(new ResultViewModel<List<Pedido>>(pedidos));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Pedido>("04x01 - Falha interna no servidor"));
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
                return Ok(new ResultViewModel<Pedido>(pedido));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Pedido>("04x02- Falha interna no servidor"));
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
                        Produto = produto,
                        Quantidade = itemVM.Quantidade
                    };

                    pedido.Itens.Add(item);
                    //await context.PedidosItens.AddAsync(item);
                }

                await context.Pedidos.AddAsync(pedido);

                foreach ( var pedidoItem in pedido.Itens )
                {
                    pedidoItem.IdPedido = pedido.Id;
                }

                //remover item do estoque
                foreach (var itemVM in vm.Itens)
                {
                    var removerEstoque = await context.Estoque.FirstOrDefaultAsync(x => x.Produto.Id == itemVM.IdProduto);
                    removerEstoque.Quantidade -= itemVM.Quantidade;
                    context.Estoque.Update(removerEstoque);
                }

                await context.SaveChangesAsync();
                return Created($"v1/pedidos/{pedido.Id}", new ResultViewModel<Pedido>(pedido));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x03 - Falha interna no servidor"));
            }
        }
        //[HttpPut("({id:int})")]
        //public async Task<IActionResult> PutAsync([FromServices] MerceariaDataContext context, [FromRoute] int id, [FromQuery] ProdutoViewModel vm)
        //{
        //    try
        //    {
        //        var produto = await context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
        //        if (produto == null)
        //            return NotFound(new ResultViewModel<Produto>("Produto não encontrado"));

        //        produto.NomeProduto = vm.NomeProduto;
        //        produto.PrecoUnit = vm.PrecoUnit;

        //        context.Produtos.Update(produto);
        //        await context.SaveChangesAsync();

        //        return Ok(new ResultViewModel<ProdutoViewModel>(vm));
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x04 - Não voi possivel alterar o produto"));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x05 - Falha interna no servidor"));
        //    }
        //}
        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> DeleteAsync([FromServices] MerceariaDataContext context, [FromRoute] int id)
        //{
        //    try
        //    {
        //        var produto = await context.Produtos.FirstOrDefaultAsync(x => x.Id == id);
        //        if (produto == null)
        //            return NotFound(new ResultViewModel<Produto>("Produto não localizado"));
        //        context.Produtos.Remove(produto);
        //        await context.SaveChangesAsync();

        //        return Ok(new ResultViewModel<Produto>(produto));
        //    }
        //    catch (DbUpdateException)
        //    {
        //        return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x06 - Não voi possivel excluir o produto"));
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, new ResultViewModel<ClienteViewModel>("02x07 - Falha interna no servidor"));
        //    }
        //}
    }
}
