using Blog.ViewModels;
using MERCEARIA.Data;
using MERCEARIA.Models;
using MERCEARIA.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MERCEARIA.Controllers
{
    [ApiController]
    [Route("v1/clientes")]
    public class ClienteController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context)
        {
            try
            {
                var clientes = await context.Clientes.ToListAsync();
                if (clientes == null)
                {
                    return NotFound(new ResultViewModel<Cliente>("Não existem clientes cadastrados"));
                }

                return Ok(new ResultViewModel<List<Cliente>>(clientes));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x01 - Falha interna no servidor"));
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromServices] MerceariaDataContext context, [FromRoute] int id)
        {
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (cliente == null)
                {
                    return NotFound(new ResultViewModel<ClienteViewModel>("Cliente não encontrado"));
                }


                return Ok(new ResultViewModel<Cliente>(cliente));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x02 - Falha interna no servidor"));
            }
        }
        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromServices] MerceariaDataContext context, [FromQuery] ClienteViewModel cliente)
        {
            try
            {
                Cliente clienteNovo = new Cliente();
                clienteNovo.Id = 0;
                clienteNovo.NomeCliente = cliente.NomeCliente;
                clienteNovo.Email = cliente.Email;
                clienteNovo.DataNascimento = cliente.DataNascimento;
                clienteNovo.Ativo = cliente.Ativo;

                await context.Clientes.AddAsync(clienteNovo);
                await context.SaveChangesAsync();

                return Created($"v1/clientes/{clienteNovo.Id}", new ResultViewModel<ClienteViewModel>(cliente));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x03 - Falha interna no servidor"));
            }
        }
        [HttpPut("({id:int})")]
        public async Task<IActionResult> PutAsync([FromServices] MerceariaDataContext context, [FromRoute] int id ,[FromQuery] ClienteViewModel vm)
        {
            try
            {
                var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (cliente == null)
                    return NotFound(new ResultViewModel<ClienteViewModel>("Cliente não encontrado"));
                cliente.NomeCliente = vm.NomeCliente;
                cliente.Email = vm.Email;
                cliente.DataNascimento = vm.DataNascimento;
                cliente.Ativo = vm.Ativo;
                

                context.Clientes.Update(cliente);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<ClienteViewModel>(vm));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x04 - Não voi possivel alterar o cliente"));
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
                var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
                if (cliente == null)
                    return NotFound(new ResultViewModel<ClienteViewModel>("Cliente não localizado"));
                context.Clientes.Remove(cliente);
                await context.SaveChangesAsync();
                var vm = new ClienteViewModel()
                {
                    NomeCliente = cliente.NomeCliente,
                    Email = cliente.Email,
                    DataNascimento = cliente.DataNascimento,
                    Ativo = cliente.Ativo
                };
                return Ok(new ResultViewModel<ClienteViewModel>(vm));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x06 - Não voi possivel excluir o cliente"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<ClienteViewModel>("01x07 - Falha interna no servidor"));
            }
        }
}
}
